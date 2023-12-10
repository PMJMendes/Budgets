using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Routing;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorRouter
{
	public partial class Switch : ComponentBase, IDisposable
	{
		private static void CreateCascadingValue<TValue>(RenderTreeBuilder builder, int seq, string name, TValue value, RenderFragment child)
		{
			builder.OpenComponent<CascadingValue<TValue>>(seq);
			builder.AddAttribute(seq + 1, "Name", name);
			builder.AddAttribute(seq + 2, "Value", value);
			builder.AddAttribute(seq + 3, "ChildContent", (RenderFragment)(builder2 => builder2.AddContent(seq++, child)));
			builder.CloseComponent();
		}

		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			base.BuildRenderTree(builder);
			CreateCascadingValue(builder, 0, "SwitchInstance", this, ChildContent);
			CreateCascadingValue(builder, 4, "RouteParameters", parameters, currentFragment);
		}

		[Parameter] public RenderFragment ChildContent { get; set; }
		[Parameter] public EventCallback<RouteMatchedEventArgs> OnMatch { get; set; }

		private readonly RouteTable routes = new RouteTable();
		private string location = "";
		private RenderFragment currentFragment;
		private IDictionary<string, object> parameters;
		private bool firstMatched;
		private RouteContext context;

		static readonly char[] queryOrHashStartChar = { '?', '#' };
		private async Task LocationChanged(object sender, LocationChangedEventArgs e)
		{
			location = e.Location;
			UpdateRouteContext();
			await SwitchContent();
		}
		private EventHandler<LocationChangedEventArgs> LocationHandler;

		private void UpdateRouteContext()
		{
			var path = NaviManager.ToBaseRelativePath(NaviManager.Uri);
			path = "/" + StringUntilAny(path, queryOrHashStartChar);
			context = new RouteContext(path);
		}

		private string StringUntilAny(string str, char[] chars)
		{
			var firstIndex = str.IndexOfAny(chars);
			return firstIndex < 0 ? str : str.Substring(0, firstIndex);
		}

		private async Task<bool> SwitchContent(string id = null, RouteEntry entry = null)
		{
			if (id == null || entry == null)
			{
				routes.Route(context);
			}
			else
			{
				routes.Route(context, id, entry);
			}

			if (context.Fragment != null)
			{
				currentFragment = context.Fragment;
				parameters = context.Parameters;

				var args = new RouteMatchedEventArgs(location, context.TemplateText, context.Segments, parameters, context.Fragment);
				await OnMatch.InvokeAsync(args);

				await InvokeAsync(() => StateHasChanged());

				return true;
			}

			return false;
		}

		protected override void OnInitialized()
		{
			LocationHandler = async (object s, LocationChangedEventArgs e) => await LocationChanged(s, e);
			location = NaviManager.Uri;
			NaviManager.LocationChanged += LocationHandler;
			UpdateRouteContext();

			base.OnInitialized();
		}

		public async Task RegisterRoute(string id, RenderFragment fragment, string template)
		{
			var entry = routes.Add(id, template, fragment);
			if (!firstMatched)
			{
				firstMatched = await SwitchContent(id, entry);
			}
		}

		public void UnregisterRoute(string id)
		{
			routes.Remove(id);
		}

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				await NavigationInterception.EnableNavigationInterceptionAsync();
			}
		}

		public void Dispose()
		{
			NaviManager.LocationChanged -= LocationHandler;
		}

		[Inject] private INavigationInterception NavigationInterception { get; set; }
		[Inject] private NavigationManager NaviManager { get; set; }
	}
}

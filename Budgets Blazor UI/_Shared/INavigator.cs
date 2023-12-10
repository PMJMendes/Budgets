using Microsoft.Extensions.Localization;
using MudBlazor;

namespace Krypton.Budgets.Blazor.UI._Shared;

public interface INavigator
{
    T? GetState<T>() where T : class;

    Task SetBreadCrumbs(List<BreadcrumbItem> crumbs);
    Task SetBreadCrumbs(string[] segments, IStringLocalizer loc);

    void NavigateWithState(string url, object state);
    Task NavigateWithStateAsync(string url, object state);
    Task RefreshWithStateAsync(object state);
    Func<Task> StateHasChangedAsync { get; set; }
}

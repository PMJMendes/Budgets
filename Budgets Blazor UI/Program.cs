using Krypton.Budgets.Blazor.APIClient._Hosting;
using Krypton.Budgets.Blazor.UI;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor;
using MudBlazor.Services;
using SoloX.BlazorJsonLocalization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var apiConfig = builder.Configuration.GetSection("Server");

builder.Services.AddHttpClient("WebAPI",
        client => client.BaseAddress = apiConfig?["Address"] is string addr ? new Uri(addr) : default).
    AddHttpMessageHandler(sp => sp.GetRequiredService<AuthorizationMessageHandler>()
        .ConfigureHandler(
            authorizedUrls: new[] { apiConfig?["Address"] ?? builder.HostEnvironment.BaseAddress },
            scopes: Array.Empty<string>()
        )
    );

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
    .CreateClient("WebAPI"));

builder.Services.AddOidcAuthentication(options =>
{
    builder.Configuration.Bind("OIDC", options.ProviderOptions);

    options.ProviderOptions.DefaultScopes.Clear();
    foreach (var scope in "openid profile email roles".Split(' '))
    {
        options.ProviderOptions.DefaultScopes.Add(scope);
    }

    options.ProviderOptions.RedirectUri = "authentication/login-callback";
    options.ProviderOptions.PostLogoutRedirectUri = "authentication/logout-callback";

    options.ProviderOptions.ResponseType = "code";
});

builder.Services.AddAPIServices();

builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomCenter;
});

builder.Services.AddJsonLocalization(builder => builder.UseEmbeddedJson(options =>
{
    options.RootNameSpaceResolver = _ => typeof(App).Namespace!;
    options.NamingPolicy = (path, culture) =>
        $"{path.Replace('/', '.')}.razor{(string.IsNullOrEmpty(culture) ? string.Empty : $"-{culture}")}.json";
}));

await builder.Build().RunAsync();

using Microsoft.Extensions.DependencyInjection;
using Krypton.Budgets.Blazor.APIClient._Base;

namespace Krypton.Budgets.Blazor.APIClient._Hosting;

public static class ServiceCompositionStatic
{
    public static IServiceCollection AddAPIServices(this IServiceCollection services)
    {
        foreach (var service in AppDomain.CurrentDomain.GetAssemblies().
            SelectMany(s => s.GetTypes()).
            Where(t => !t.IsAbstract && typeof(ServiceBase).IsAssignableFrom(t)))
        {
            services.AddScoped(service);
            services.AddScoped(typeof(ServiceBase), service);
        }

        return services;
    }
}

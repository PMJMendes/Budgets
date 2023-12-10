using Krypton.Budgets.Domain._Ports.Persistence;
using Krypton.Budgets.Persistence.Implementation;
using Microsoft.Extensions.DependencyInjection;
using NHibernate;

namespace Krypton.Budgets.Persistence.Hosting;

public static class PersistenceCompositionStatic
{
    public static IServiceCollection AddBudgetsPersistence(this IServiceCollection services)
    {
        return services.
            AddSingleton<IPersistenceHosting, NHibernateBridge>().
            AddSingleton(sp => ((NHibernateBridge)sp.GetRequiredService<IPersistenceHosting>()).GetFactory()).
            AddScoped<IInterceptor, EntityManagementInterceptor>().
            AddScoped(sp => sp.GetRequiredService<ISessionFactory>().
                WithOptions().
                Interceptor(sp.GetRequiredService<IInterceptor>()).
                OpenSession()).
            AddScoped<IPersistence, PersistenceImpl>();
    }
}

using Krypton.Budgets.Domain._Base.Interfaces;
using Krypton.Budgets.Domain._Impl.Interfaces;
using Krypton.Budgets.Domain._Ports.Persistence;
using Krypton.Budgets.Domain.Global;
using Microsoft.Extensions.DependencyInjection;

namespace Krypton.Budgets.Domain._Ports.Hosting;

public static class DomainCompositionStatic
{
    public static IServiceCollection AddBudgetsDomain(this IServiceCollection services)
    {
        services.AddSingleton<IDomainExplorer, DomainExplorerImpl>();

        var intfs = typeof(IOperation<,>).Assembly.DefinedTypes.
            Where(t =>
                t.IsInterface &&
                t != typeof(IOperation<,>) &&
                t.IsAssignableToGenericType(typeof(IOperation<,>))).
            ToHashSet();

        typeof(IOperation<,>).Assembly.DefinedTypes.Where(t =>
                   !t.IsInterface &&
                   t.IsAssignableToGenericType(typeof(IOperation<,>))).
               Select(o => (Impl: o, Intf: intfs.FirstOrDefault(i => i.IsAssignableFrom(o)))).
               Where(z => z.Intf is not null).
               ToList().
               ForEach(z => services.AddTransient(z.Intf!, z.Impl));

        intfs = typeof(IQuery<,>).Assembly.DefinedTypes.
            Where(t =>
                t.IsInterface &&
                t != typeof(IQuery<,>) &&
                t.IsAssignableToGenericType(typeof(IQuery<,>))).
            ToHashSet();

        typeof(IQuery<,>).Assembly.DefinedTypes.Where(t =>
                   !t.IsInterface &&
                   t.IsAssignableToGenericType(typeof(IQuery<,>))).
               Select(o => (Impl: o, Intf: intfs.FirstOrDefault(i => i.IsAssignableFrom(o)))).
               Where(z => z.Intf is not null).
               ToList().
               ForEach(z => services.AddTransient(z.Intf!, z.Impl));

        services.AddScoped<IContext, GlobalObject>();

        return services;
    }

    private static bool IsAssignableToGenericType(this Type givenType, Type genericType)
    {
        var interfaceTypes = givenType.GetInterfaces();

        foreach (var it in interfaceTypes)
        {
            if (it.IsGenericType && it.GetGenericTypeDefinition() == genericType)
                return true;
        }

        if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
            return true;

        if (givenType.BaseType is Type baseType)
        {
            return IsAssignableToGenericType(baseType, genericType);
        }

        return false;
    }
}

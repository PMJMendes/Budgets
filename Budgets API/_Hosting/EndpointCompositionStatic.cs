using Krypton.Budgets.API._Base;
using Krypton.Budgets.Domain._Ports.Persistence;
using Microsoft.AspNetCore.Http.Json;
using System.Text.Json.Serialization;
using MvcJsonOptions = Microsoft.AspNetCore.Mvc.JsonOptions;

namespace Krypton.Budgets.API._Hosting;

internal static class EndpointCompositionStatic
{
    public static IServiceCollection AddEndpoints(this IServiceCollection services)
    {
        services.Configure<JsonOptions>(o =>
        {
            o.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });
        services.Configure<MvcJsonOptions>(o =>
        {
            o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

        foreach (var endpoint in AppDomain.CurrentDomain.GetAssemblies().
            SelectMany(s => s.GetTypes()).
            Where(t => !t.IsAbstract && typeof(EndpointBase).IsAssignableFrom(t)))
        {
            services.AddScoped(endpoint);
            services.AddScoped(typeof(EndpointBase), endpoint);
        }

        return services;
    }

    public static void UseEndpoints(this WebApplication app, IDomainExplorer explorer)
    {
        var root = app.MapGroup("api").WithOpenApi();

        using (var scope = app.Services.CreateScope())
        {
            foreach (var group in scope.ServiceProvider.GetServices<EndpointBase>().
                Where(t => t.GetType().Namespace is not null).
                GroupBy(t => explorer.GetEntitySubdomain(t.GetType())).
                OrderBy(g => g.Key == "Global" ? "" : g.Key))
            {
                var map = root.MapGroup(group.Key.ToLower()).WithTags(group.Key);

                foreach (var endpoint in group)
                {
                    var type = endpoint.GetType();

                    var verb = type switch
                    {
                        Type t when t.IsAssignableToGenericType(typeof(GetEndpointBase<,,>)) => new[] { HttpMethods.Get },
                        Type t when t.IsAssignableToGenericType(typeof(PostEndpointBase<,,>)) => new[] { HttpMethods.Post },
                        _ => throw new Exception($"Invalid endpoint: {endpoint.GetType()}"),
                    };

                    map.MapMethods(type.Name, verb, endpoint.Route).
                        Produces(200, type.BaseType!.GenericTypeArguments[1]);
                }
            }
        }
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
            return baseType.IsAssignableToGenericType(genericType);
        }

        return false;
    }
}

using Krypton.Budgets.Domain._Base.Attributes;
using Krypton.Budgets.Domain._Base.Interfaces;
using Krypton.Budgets.Domain._Impl.Exceptions;
using Krypton.Budgets.Domain._Impl.Interfaces;
using Krypton.Budgets.Domain._Impl.Utils;
using Krypton.Budgets.Domain._Ports.Security;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace Krypton.Budgets.Domain._Base.Services;

internal abstract class BaseService<K> where K : BaseService<K>
{
    protected readonly IContext context;
    protected readonly ILogger<K> logger;

    public BaseService(IContext context, ILogger<K> logger)
    {
        this.context = context;
        this.logger = logger;
    }

    protected abstract void AssertPermissions();

    protected void ValidateArgs(IArguments? maybeArgs)
    {
        if (maybeArgs is IArguments args)
        {

            logger.LogDebug("Validating arguments: {@Args}", args);

            var allInterfaces = args.GetType().GetInterfaces().Where(t => typeof(IArguments).IsAssignableFrom(t));
            var type = allInterfaces.
                Except(allInterfaces.SelectMany(i => i.GetInterfaces())).
                FirstOrDefault() ?? args.GetType();

            var groups = type.GetProperties().
                SelectMany(prop => ValidateProp(prop, args)).
                GroupBy(p => p.Property, p => p.Errors).
                Select(g => new { PropRef = g.Key, Values = g.SelectMany(e => e) });

            if (groups.Any())
            {
                var ex = new InvalidArgsException(groups.
                    SelectMany(g => g.Values, (p, e) => KeyValuePair.Create(e, p.PropRef)).
                    Select<KeyValuePair<Enum, PropertyInfo>, (Enum, PropertyInfo)>(p => new(p.Key, p.Value)), "");

                logger.LogWarning(ex, "Argument validation failed");

                throw ex;
            };
        }
        else
        {
            logger.LogWarning("Null arguments passed to validation");
        }

        logger.LogInformation("Arguments validated");
    }

    protected async Task<T> GetTarget<T>(ITargetArgs? args) where T : IEntity =>
        await context.Persistence.GetAsync<T>(args?.TargetId ?? Guid.Empty) ??
            throw BuildTargetNotFound<T>(args);

    protected void AssertIsLoggedIn()
    {
        if (!context.Security.IsLoggedIn())
        {
            var ex = new AuthenticationRequiredException();
            logger.LogWarning(ex, "Authentication required");
            throw ex;
        }
    }

    protected void AssertIsAdmin()
    {
        AssertIsLoggedIn();

        if (!context.Security.HasSecurityLevel(SecurityLevel.Admin))
        {
            var ex = new PermissionDeniedException();
            logger.LogWarning(ex, "Admin required");
            throw ex;
        }
    }

    protected void AssertIsProducer()
    {
        AssertIsLoggedIn();

        if (!context.Security.HasSecurityLevel(SecurityLevel.Producer))
        {
            var ex = new PermissionDeniedException();
            logger.LogWarning(ex, "Producer required");
            throw ex;
        }
    }

    protected void AssertIsAccounting()
    {
        AssertIsLoggedIn();

        if (!context.Security.HasSecurityLevel(SecurityLevel.Accounting) &&
            !context.Security.HasSecurityLevel(SecurityLevel.Producer))
        {
            var ex = new PermissionDeniedException();
            logger.LogWarning(ex, "Accounting required");
            throw ex;
        }
    }

    protected InvalidArgsException BuildInvalidArgs(PropertyInfo propRef, Enum tag, string msg)
    {
        var ex = new InvalidArgsException(propRef, tag, msg);
        logger.LogError(ex, "{msg}", msg);
        return ex;
    }

    protected Exception BuildUnexpectedValidationFailure()
    {
        var ex = new Exception("Unexpected validation false positive");
        logger.LogError(ex, "Unexpected null argument during execution");
        return ex;
    }

    protected TargetNotFoundException BuildTargetNotFound<T>(ITargetArgs? args)
    {
        var ex = new TargetNotFoundException(TypeHelper.GetProperty((ITargetArgs c) => c.TargetId), typeof(T));
        logger.LogWarning(ex, "Target {Entity} with Id {Id} not found", nameof(T), args?.TargetId ?? Guid.Empty);
        return ex;
    }

    private static IEnumerable<(PropertyInfo Property, IEnumerable<Enum> Errors)> ValidateProp(PropertyInfo prop, IArguments args)
    {
        return prop.GetCustomAttributes<DomainAttribute>().
                SelectMany(attr => attr.IsValid(prop, prop.GetValue(args)));
    }
}

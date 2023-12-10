using Krypton.Budgets.API._Impl;
using Krypton.Budgets.Domain._Base.Exceptions;
using Krypton.Budgets.Domain._Base.Interfaces;
using Krypton.Budgets.Domain._Impl.Exceptions;

namespace Krypton.Budgets.API._Base;

internal abstract class EndpointBase
{
    public abstract Delegate Route { get; }
}

internal abstract class EndpointBase<K> : EndpointBase where K : EndpointBase<K>
{
    private static readonly Type[] BAD_REQUEST_EXCEPTIONS = new[] {
            typeof(InvalidArgsException),
            typeof(TargetNotFoundException),
            typeof(UniqueException)
        };

    protected readonly HttpContext? context;
    protected readonly ILogger<K> logger;

    protected EndpointBase(IHttpContextAccessor accessor, ILogger<K> logger)
    {
        context = accessor.HttpContext;
        this.logger = logger;
    }

    protected abstract Task<IResult> InInnerScope(Func<Task<IResult>> func);

    protected Task<IResult> InRequestScope(Func<Task<IResult>> func)
    {
        using (logger.BeginScope(("Correlation Id", context?.TraceIdentifier ?? Guid.Empty.ToString())))
        using (logger.BeginScope(("API User", context?.User?.Identity?.Name ?? "(unknown)")))
        {
            return InInnerScope(func);
        }
    }

    protected async Task<IResult> CallDomainOpAsync<T, U, V>(IOperation<T, U> op, T? args, Func<U, V> resultFactory) where T : IArguments where U : IOpResults where V : U
    {
        try
        {
            logger.LogInformation("Invoking domain operation");

            var result = await op.Execute(args, resultFactory);

            logger.LogInformation("Domain call successful");
            logger.LogDebug("Domain returned: {@Result}", result);

            return Results.Ok(result);
        }
        catch (AuthenticationRequiredException)
        {
            logger.LogDebug("Unauthorized by domain");

            return Results.Unauthorized();
        }
        catch (PermissionDeniedException)
        {
            logger.LogDebug("Forbidden by domain");

            return Results.Forbid();
        }
        catch (DomainException ex)
        {
            return HandleDomainException(ex);
        }
        catch (Exception ex)
        {
            return HandleUnknownException(ex);
        }
    }

    protected async Task<IResult> CallDomainQueryAsync<T, U, V>(IQuery<T, U> query, T? parms, Func<U, V> itemFactory) where T : IArguments where U : class, IQueryResultItem where V : U
    {
        try
        {
            logger.LogInformation("Invoking domain query");

            var result = await query.Fetch(parms, itemFactory).ToListAsync();

            logger.LogInformation("Domain call successful");
            logger.LogDebug("Domain returned: {@Result}", result);

            return Results.Ok(result);
        }
        catch (AuthenticationRequiredException)
        {
            logger.LogDebug("Unauthorized by domain");

            return Results.Unauthorized();
        }
        catch (PermissionDeniedException)
        {
            logger.LogDebug("Forbidden by domain");

            return Results.Forbid();
        }
        catch (DomainException ex)
        {
            return HandleDomainException(ex);
        }
        catch (Exception ex)
        {
            return HandleUnknownException(ex);
        }
    }

    private IResult HandleDomainException(DomainException ex)
    {
        var status = BAD_REQUEST_EXCEPTIONS.Contains(ex.GetType()) ? 400 : 500;

        var errors = new ErrorList(status, ex.Errors.Select(t =>
            new Error(t.PropRef.Name, t.Tag.ToString(), ex.Message)));

        logger.LogWarning(ex, "Domain invocation exception: status {Status}", status);

        return Results.Json(errors, statusCode: status, contentType: "application/problem+json");
    }

    private IResult HandleUnknownException(Exception ex)
    {
        var errors = new ErrorList(500, new Error[]
        {
            new Error("", "UNKNOWN_ERROR", ex.Message)
        });

        logger.LogWarning(ex, "Domain invocation exception: status {Status}", 500);

        return Results.Json(errors, statusCode: 500, contentType: "application/problem+json");
    }
}

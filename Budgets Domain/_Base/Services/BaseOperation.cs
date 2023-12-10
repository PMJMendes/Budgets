using Krypton.Budgets.Domain._Base.Interfaces;
using Krypton.Budgets.Domain._Impl.Interfaces;
using Microsoft.Extensions.Logging;

namespace Krypton.Budgets.Domain._Base.Services;

internal abstract class BaseOperation<K, T, U> : BaseService<K>, IOperation<T, U>
    where K : BaseOperation<K, T, U>
    where T : IArguments
    where U : IOpResults
{
    protected BaseOperation(IContext context, ILogger<K> logger) : base(context, logger) { }

    public Task<V> Execute<V>(T? args, Func<U, V> resultsFactory) where V : U
    {
        using (logger.BeginScope(("Correlation Id", context.DomainClient.CorrelationId)))
        using (logger.BeginScope(("Current User", context.Security.LoginTag)))
        using (logger.BeginScope(("Domain Operation", GetType().Name)))
        {
            var user = context.CurrentUser;

            logger.LogDebug("Asserting permissions");

            AssertPermissions();

            logger.LogInformation("General permission granted");

            ValidateArgs(args);

            logger.LogDebug("Preparing results async task");

            Task<V> task = InnerExecute(args, resultsFactory);

            user?.UserLoggedIn();

            logger.LogInformation("Async task ready, domain returning");

            return task;
        }
    }

    protected abstract Task<V> InnerExecute<V>(T? args, Func<U, V> resultsFactory) where V : U;

    protected readonly record struct EmptyResults : IOpResults
    {
        public static readonly EmptyResults Instance = new();
    }
}

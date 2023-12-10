using Krypton.Budgets.Domain._Base.Interfaces;
using Krypton.Budgets.Domain._Impl.Interfaces;
using Microsoft.Extensions.Logging;

namespace Krypton.Budgets.Domain._Base.Services;

internal abstract class BaseQuery<K, T, U> : BaseService<K>, IQuery<T, U>
    where K : BaseQuery<K, T, U>
    where T : IArguments
    where U : IQueryResultItem
{
    protected BaseQuery(IContext context, ILogger<K> logger) : base(context, logger) { }

    public IAsyncEnumerable<V> Fetch<V>(T? args, Func<U, V> itemFactory) where V : U
    {
        using (logger.BeginScope(("Correlation Id", context.DomainClient.CorrelationId)))
        using (logger.BeginScope(("Current User", context.Security.LoginTag)))
        using (logger.BeginScope(("Domain Query", GetType().Name)))
        {
            logger.LogDebug("Asserting permissions");

            AssertPermissions();

            logger.LogInformation("General permission granted");

            ValidateArgs(args);

            logger.LogDebug("Preparing results async enumerable");

            IAsyncEnumerable<V> asyncEnumerable = InnerFetch(args, itemFactory);

            context.CurrentUser?.UserLoggedIn();

            logger.LogInformation("Async enumerable ready, domain returning");

            return asyncEnumerable;
        }
    }

    protected abstract IAsyncEnumerable<V> InnerFetch<V>(T? args, Func<U, V> itemFactory) where V : U;
}

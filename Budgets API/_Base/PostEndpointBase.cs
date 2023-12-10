using Krypton.Budgets.API._Impl;

namespace Krypton.Budgets.API._Base;

internal abstract class PostEndpointBase<K, R, A> : EndpointBase<K>
    where K : PostEndpointBase<K, R, A>
{
    protected PostEndpointBase(IHttpContextAccessor accessor, ILogger<K> logger) :
        base(accessor, logger)
    { }

    protected abstract Task<IResult> PostAsync(A? args);

    public override Delegate Route => (K _this, A? args) => _this.PostAsync(args);

    protected override sealed Task<IResult> InInnerScope(Func<Task<IResult>> func)
    {
        using (logger.BeginScope(("POST Endpoint", GetType().Name)))
        {
            return func();
        }
    }
}

internal abstract class PostEndpointBase<K, R> : PostEndpointBase<K, R, EmptyArgs?>
    where K : PostEndpointBase<K, R>
{
    protected PostEndpointBase(IHttpContextAccessor accessor, ILogger<K> logger) :
        base(accessor, logger)
    { }

    protected abstract Task<IResult> PostAsync();

    public override Delegate Route => (K _this) => _this.PostAsync();

    protected override sealed Task<IResult> PostAsync(EmptyArgs? _) => PostAsync();
}

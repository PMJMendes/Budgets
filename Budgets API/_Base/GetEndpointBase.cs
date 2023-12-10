using Krypton.Budgets.API._Impl;

namespace Krypton.Budgets.API._Base;

internal abstract class GetEndpointBase<K, R, A> : EndpointBase<K>
    where K : GetEndpointBase<K, R, A>
{
    protected GetEndpointBase(IHttpContextAccessor accessor, ILogger<K> logger) :
        base(accessor, logger)
    { }

    protected abstract Task<IResult> GetAsync(A args);

    public override Delegate Route => (K _this, [AsParameters] A args) =>
        _this.GetAsync(args);

    protected override sealed Task<IResult> InInnerScope(Func<Task<IResult>> func)
    {
        using (logger.BeginScope(("GET Endpoint", GetType().Name)))
        {
            return func();
        }
    }
}

internal abstract class GetEndpointBase<K, R> : GetEndpointBase<K, R, EmptyQueryArgs?>
    where K : GetEndpointBase<K, R>
{
    protected GetEndpointBase(IHttpContextAccessor accessor, ILogger<K> logger) :
        base(accessor, logger)
    { }

    protected abstract Task<IResult> GetAsync();

    public override Delegate Route => (K _this) => _this.GetAsync();
    protected override sealed Task<IResult> GetAsync(EmptyQueryArgs? args) => GetAsync();
}

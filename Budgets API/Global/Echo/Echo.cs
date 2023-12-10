using Krypton.Budgets.API._Base;
using Krypton.Budgets.API._Impl;
using Krypton.Budgets.Domain.Global.Echo;

namespace Krypton.Budgets.API.Global.Echo;

internal class Echo : GetEndpointBase<Echo, EchoItem>
{
    private readonly IEcho query;

    public Echo(IEcho query, IHttpContextAccessor accessor, ILogger<Echo> logger) : base(accessor, logger)
    {
        this.query = query;
    }

    protected override Task<IResult> GetAsync() => InRequestScope(() =>
        CallDomainQueryAsync(query, EmptyArgs.Instance, r => new EchoItem(r)));
}

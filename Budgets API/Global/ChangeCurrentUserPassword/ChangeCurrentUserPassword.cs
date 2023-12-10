using Krypton.Budgets.API._Base;
using Krypton.Budgets.API._Impl;
using Krypton.Budgets.Domain.Global.ChangeCurrentUserPassword;

namespace Krypton.Budgets.API.Global.ChangeCurrentUserPassword;

internal class ChangeCurrentUserPassword : PostEndpointBase<ChangeCurrentUserPassword, EmptyResults>
{
    private readonly IChangeCurrentUserPassword op;

    public ChangeCurrentUserPassword(IChangeCurrentUserPassword op, IHttpContextAccessor accessor, ILogger<ChangeCurrentUserPassword> logger) : base(accessor, logger)
    {
        this.op = op;
    }

    protected override Task<IResult> PostAsync() => InRequestScope(() =>
            CallDomainOpAsync(op, EmptyArgs.Instance, _ => EmptyResults.Instance));
}

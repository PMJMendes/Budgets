using Krypton.Budgets.API._Base;
using Krypton.Budgets.API._Impl;
using Krypton.Budgets.Domain.Budgets.Budget_.AssignManager;

namespace Krypton.Budgets.API.Budgets.Budget.AssignManager;

internal class AssignManager : PostEndpointBase<AssignManager, EmptyResults, AssignManagerArgs>
{
    private readonly IAssignManager op;

    public AssignManager(IAssignManager op, IHttpContextAccessor accessor, ILogger<AssignManager> logger) : base(accessor, logger)
    {
        this.op = op;
    }

    protected override Task<IResult> PostAsync(AssignManagerArgs? args) => InRequestScope(() =>
        CallDomainOpAsync(op, args, _ => EmptyResults.Instance));
}

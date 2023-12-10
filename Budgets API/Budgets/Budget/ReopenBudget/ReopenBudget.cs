using Krypton.Budgets.API._Base;
using Krypton.Budgets.API._Impl;
using Krypton.Budgets.Domain.Budgets.Budget_.ReopenBudget;

namespace Krypton.Budgets.API.Budgets.Budget.ReopenBudget;

internal class ReopenBudget : PostEndpointBase<ReopenBudget, ReopenBudgetResults, TargetArgs>
{
    private readonly IReopenBudget op;

    public ReopenBudget(IReopenBudget op, IHttpContextAccessor accessor, ILogger<ReopenBudget> logger) : base(accessor, logger)
    {
        this.op = op;
    }

    protected override Task<IResult> PostAsync(TargetArgs? args) => InRequestScope(() =>
        CallDomainOpAsync(op, args, r => new ReopenBudgetResults(r)));
}

using Krypton.Budgets.API._Base;
using Krypton.Budgets.API._Impl;
using Krypton.Budgets.Domain.Budgets.Budget_.LockBudget;

namespace Krypton.Budgets.API.Budgets.Budget.LockBudget;

internal class LockBudget : PostEndpointBase<LockBudget, LockBudgetResults, TargetArgs>
{
    private readonly ILockBudget op;

    public LockBudget(ILockBudget op, IHttpContextAccessor accessor, ILogger<LockBudget> logger) : base(accessor, logger)
    {
        this.op = op;
    }

    protected override Task<IResult> PostAsync(TargetArgs? args) => InRequestScope(() =>
        CallDomainOpAsync(op, args, r => new LockBudgetResults(r)));
}

using Krypton.Budgets.API._Base;
using Krypton.Budgets.API._Impl;
using Krypton.Budgets.Domain.Budgets.Budget_.UnlockBudget;

namespace Krypton.Budgets.API.Budgets.Budget.UnlockBudget;

internal class UnlockBudget : PostEndpointBase<UnlockBudget, UnlockBudgetResults, TargetArgs>
{
    private readonly IUnlockBudget op;

    public UnlockBudget(IUnlockBudget op, IHttpContextAccessor accessor, ILogger<UnlockBudget> logger) : base(accessor, logger)
    {
        this.op = op;
    }

    protected override Task<IResult> PostAsync(TargetArgs? args) => InRequestScope(() =>
        CallDomainOpAsync(op, args, r => new UnlockBudgetResults(r)));
}

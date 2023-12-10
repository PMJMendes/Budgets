using Krypton.Budgets.API._Base;
using Krypton.Budgets.API._Impl;
using Krypton.Budgets.Domain.Budgets.Budget_.CloseBudget;

namespace Krypton.Budgets.API.Budgets.Budget.CloseBudget;

internal class CloseBudget : PostEndpointBase<CloseBudget, CloseBudgetResults, TargetArgs>
{
    private readonly ICloseBudget op;

    public CloseBudget(ICloseBudget op, IHttpContextAccessor accessor, ILogger<CloseBudget> logger) : base(accessor, logger)
    {
        this.op = op;
    }

    protected override Task<IResult> PostAsync(TargetArgs? args) => InRequestScope(() =>
        CallDomainOpAsync(op, args, r => new CloseBudgetResults(r)));
}

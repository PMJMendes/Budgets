using Krypton.Budgets.API._Base;
using Krypton.Budgets.API._Impl;
using Krypton.Budgets.Domain.Budgets.Budget_.DeleteBudget;

namespace Krypton.Budgets.API.Budgets.Budget.DeleteBudget;

internal class DeleteBudget : PostEndpointBase<DeleteBudget, EmptyResults, TargetArgs>
{
    private readonly IDeleteBudget op;

    public DeleteBudget(IDeleteBudget op, IHttpContextAccessor accessor, ILogger<DeleteBudget> logger) : base(accessor, logger)
    {
        this.op = op;
    }

    protected override Task<IResult> PostAsync(TargetArgs? args) => InRequestScope(() =>
        CallDomainOpAsync(op, args, _ => EmptyResults.Instance));
}

using Krypton.Budgets.API._Base;
using Krypton.Budgets.API._Impl;
using Krypton.Budgets.Domain.Budgets.Budget_.BudgetDetails;

namespace Krypton.Budgets.API.Budgets.Budget.BudgetDetails;

internal class BudgetDetails : GetEndpointBase<BudgetDetails, BudgetDetailsItem, TargetQueryArgs>
{
    private readonly IBudgetDetails query;

    public BudgetDetails(IBudgetDetails query, IHttpContextAccessor accessor, ILogger<BudgetDetails> logger) : base(accessor, logger)
    {
        this.query = query;
    }

    protected override Task<IResult> GetAsync(TargetQueryArgs args) => InRequestScope(() =>
        CallDomainQueryAsync(query, args, r => new BudgetDetailsItem(r)));
}

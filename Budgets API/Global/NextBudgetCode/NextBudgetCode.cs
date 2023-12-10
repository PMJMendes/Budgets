using Krypton.Budgets.API._Base;
using Krypton.Budgets.Domain.Global.NextBudgetCode;

namespace Krypton.Budgets.API.Global.NextBudgetCode;

internal class NextBudgetCode : GetEndpointBase<NextBudgetCode, NextBudgetCodeItem, NextBudgetCodeArgs>
{
    private readonly INextBudgetCode query;

    public NextBudgetCode(INextBudgetCode query, IHttpContextAccessor accessor, ILogger<NextBudgetCode> logger) : base(accessor, logger)
    {
        this.query = query;
    }

    protected override Task<IResult> GetAsync(NextBudgetCodeArgs args) => InRequestScope(() =>
        CallDomainQueryAsync(query, args, r => new NextBudgetCodeItem(r)));
}

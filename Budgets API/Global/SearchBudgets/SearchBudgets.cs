using Krypton.Budgets.API._Base;
using Krypton.Budgets.Domain.Global.SearchBudgets;

namespace Krypton.Budgets.API.Global.SearchBudgets;

internal class SearchBudgets : GetEndpointBase<SearchBudgets, SearchBudgetsItem, SearchBudgetsArgs>
{
    private readonly ISearchBudgets query;

    public SearchBudgets(ISearchBudgets query, IHttpContextAccessor accessor, ILogger<SearchBudgets> logger) : base(accessor, logger)
    {
        this.query = query;
    }

    protected override Task<IResult> GetAsync(SearchBudgetsArgs args) => InRequestScope(() =>
        CallDomainQueryAsync(query, args, r => new SearchBudgetsItem(r)));
}

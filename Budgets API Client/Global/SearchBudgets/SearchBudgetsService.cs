using Krypton.Budgets.Blazor.APIClient._Base;
using Krypton.Budgets.Blazor.APIClient._Common;

namespace Krypton.Budgets.Blazor.APIClient.Global.SearchBudgets;

public class SearchBudgetsService : GetServiceBase<SearchBudgetsArgs, SearchBudgetsItem>
{
    private static readonly string URL = "global/SearchBudgets";

    public SearchBudgetsService(HttpClient client) : base(client, URL) { }

    public async Task<SafeResult<IEnumerable<SearchBudgetsItem?>>> SearchBudgetsAsync(SearchBudgetsArgs args) =>
        await GetAsync(args);
}

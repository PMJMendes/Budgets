using Krypton.Budgets.Blazor.APIClient._Base;
using Krypton.Budgets.Blazor.APIClient._Common;

namespace Krypton.Budgets.Blazor.APIClient.Global.NextBudgetCode;

public class NextBudgetCodeService : GetServiceBase<NextBudgetCodeArgs, NextBudgetCodeItem>
{
    private static readonly string URL = "global/NextBudgetCode";

    public NextBudgetCodeService(HttpClient client) : base(client, URL) { }

    public async Task<SafeResult<IEnumerable<NextBudgetCodeItem?>>> NextBudgetCodeAsync(NextBudgetCodeArgs args) =>
        await GetAsync(args);
}

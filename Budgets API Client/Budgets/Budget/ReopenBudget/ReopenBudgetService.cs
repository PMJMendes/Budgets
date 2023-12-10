using Krypton.Budgets.Blazor.APIClient._Base;
using Krypton.Budgets.Blazor.APIClient._Common;

namespace Krypton.Budgets.Blazor.APIClient.Budgets.Budget.ReopenBudget;

public class ReopenBudgetService : PostServiceBase<TargetPostArgs, ReopenBudgetResults>
{
    private static readonly string URL = "budgets/ReopenBudget";

    public ReopenBudgetService(HttpClient client) : base(client, URL) { }

    public async Task<SafeResult<ReopenBudgetResults>> ReopenBudgetAsync(TargetPostArgs args) =>
        await PostAsync(args);
}

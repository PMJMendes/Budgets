using Krypton.Budgets.Blazor.APIClient._Base;
using Krypton.Budgets.Blazor.APIClient._Common;

namespace Krypton.Budgets.Blazor.APIClient.Budgets.Budget.UnlockBudget;

public class UnlockBudgetService : PostServiceBase<TargetPostArgs, UnlockBudgetResults>
{
    private static readonly string URL = "budgets/UnlockBudget";

    public UnlockBudgetService(HttpClient client) : base(client, URL) { }

    public async Task<SafeResult<UnlockBudgetResults>> UnlockBudgetAsync(TargetPostArgs args) =>
        await PostAsync(args);
}

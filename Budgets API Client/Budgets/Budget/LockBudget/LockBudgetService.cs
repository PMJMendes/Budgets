using Krypton.Budgets.Blazor.APIClient._Base;
using Krypton.Budgets.Blazor.APIClient._Common;

namespace Krypton.Budgets.Blazor.APIClient.Budgets.Budget.LockBudget;

public class LockBudgetService : PostServiceBase<TargetPostArgs, LockBudgetResults>
{
    private static readonly string URL = "budgets/LockBudget";

    public LockBudgetService(HttpClient client) : base(client, URL) { }

    public async Task<SafeResult<LockBudgetResults>> LockBudgetAsync(TargetPostArgs args) =>
        await PostAsync(args);
}

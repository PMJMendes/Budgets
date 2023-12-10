using Krypton.Budgets.Blazor.APIClient._Base;
using Krypton.Budgets.Blazor.APIClient._Common;

namespace Krypton.Budgets.Blazor.APIClient.Budgets.Budget.CloseBudget;

public class CloseBudgetService : PostServiceBase<TargetPostArgs, CloseBudgetResults>
{
    private static readonly string URL = "budgets/CloseBudget";

    public CloseBudgetService(HttpClient client) : base(client, URL) { }

    public async Task<SafeResult<CloseBudgetResults>> CloseBudgetAsync(TargetPostArgs args) =>
        await PostAsync(args);
}

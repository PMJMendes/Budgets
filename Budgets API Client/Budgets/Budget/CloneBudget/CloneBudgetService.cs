using Krypton.Budgets.Blazor.APIClient._Base;
using Krypton.Budgets.Blazor.APIClient._Common;
using Krypton.Budgets.Blazor.APIClient.Budgets.Budget.BudgetDetails;

namespace Krypton.Budgets.Blazor.APIClient.Budgets.Budget.CloneBudget;

public class CloneBudgetService : PostServiceBase<CloneBudgetArgs, BudgetDetailsItem>
{
    private static readonly string URL = "budgets/CloneBudget";

    public CloneBudgetService(HttpClient client) : base(client, URL) { }

    public async Task<SafeResult<BudgetDetailsItem>> CloneBudgetAsync(CloneBudgetArgs args) =>
        await PostAsync(args);
}

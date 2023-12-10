using Krypton.Budgets.Blazor.APIClient._Base;
using Krypton.Budgets.Blazor.APIClient._Common;
using Krypton.Budgets.Blazor.APIClient.Budgets.Budget.BudgetDetails;

namespace Krypton.Budgets.Blazor.APIClient.Budgets.Budget.UpdateBudget;

public class UpdateBudgetService : PostServiceBase<UpdateBudgetArgs, BudgetDetailsItem>
{
    private static readonly string URL = "budgets/UpdateBudget";

    public UpdateBudgetService(HttpClient client) : base(client, URL) { }

    public async Task<SafeResult<BudgetDetailsItem>> UpdateBudgetAsync(
        UpdateBudgetArgs args) => await PostAsync(args);
}

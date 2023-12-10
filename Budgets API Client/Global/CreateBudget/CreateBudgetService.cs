using Krypton.Budgets.Blazor.APIClient._Base;
using Krypton.Budgets.Blazor.APIClient._Common;
using Krypton.Budgets.Blazor.APIClient.Budgets.Budget.BudgetDetails;

namespace Krypton.Budgets.Blazor.APIClient.Global.CreateBudget;

public class CreateBudgetService : PostServiceBase<CreateBudgetArgs, BudgetDetailsItem>
{
    private static readonly string URL = "global/CreateBudget";

    public CreateBudgetService(HttpClient client) : base(client, URL) { }

    public async Task<SafeResult<BudgetDetailsItem>> CreateBudgetAsync(CreateBudgetArgs args) =>
        await PostAsync(args);
}

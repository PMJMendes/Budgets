using Krypton.Budgets.Blazor.APIClient._Base;
using Krypton.Budgets.Blazor.APIClient._Common;
using Krypton.Budgets.Blazor.APIClient.Budgets.Budget.ProductionDetails;

namespace Krypton.Budgets.Blazor.APIClient.Budgets.Budget.ManageBudget;

public class ManageBudgetService : PostServiceBase<ManageBudgetArgs, ProductionDetailsItem>
{
    private static readonly string URL = "budgets/ManageBudget";

    public ManageBudgetService(HttpClient client) : base(client, URL) { }

    public async Task<SafeResult<ProductionDetailsItem>> ManageBudgetAsync(
        ManageBudgetArgs args) => await PostAsync(args);
}

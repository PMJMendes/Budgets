using Krypton.Budgets.Blazor.APIClient._Base;
using Krypton.Budgets.Blazor.APIClient._Common;

namespace Krypton.Budgets.Blazor.APIClient.Budgets.Budget.BudgetDetails;

public class BudgetDetailsService : GetServiceBase<TargetQueryArgs, BudgetDetailsItem>
{
    private static readonly string URL = "budgets/BudgetDetails";

    public BudgetDetailsService(HttpClient client) : base(client, URL) { }

    public async Task<SafeResult<IEnumerable<BudgetDetailsItem?>>> BudgetDetailsAsync(TargetQueryArgs args) =>
        await GetAsync(args);
}

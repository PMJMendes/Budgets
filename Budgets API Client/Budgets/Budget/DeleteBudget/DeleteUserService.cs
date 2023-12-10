using Krypton.Budgets.Blazor.APIClient._Base;
using Krypton.Budgets.Blazor.APIClient._Common;
using Krypton.Budgets.Blazor.APIClient._Impl;

namespace Krypton.Budgets.Blazor.APIClient.Budgets.Budget.DeleteBudget;

public class DeleteBudgetService : PostServiceBase<TargetPostArgs, EmptyResults>
{
    private static readonly string URL = "budgets/DeleteBudget";

    public DeleteBudgetService(HttpClient client) : base(client, URL) { }

    public async Task<SafeResult<EmptyResults>> DeleteBudgetAsync(TargetPostArgs args) =>
        await PostAsync(args);
}

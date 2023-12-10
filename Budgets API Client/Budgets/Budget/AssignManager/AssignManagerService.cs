using Krypton.Budgets.Blazor.APIClient._Base;
using Krypton.Budgets.Blazor.APIClient._Common;
using Krypton.Budgets.Blazor.APIClient._Impl;

namespace Krypton.Budgets.Blazor.APIClient.Budgets.Budget.AssignManager;

public class AssignManagerService : PostServiceBase<AssignManagerArgs, EmptyResults>
{
    private static readonly string URL = "budgets/AssignManager";

    public AssignManagerService(HttpClient client) : base(client, URL) { }

    public async Task<SafeResult<EmptyResults>> AssignManagerAsync(AssignManagerArgs args) =>
        await PostAsync(args);
}

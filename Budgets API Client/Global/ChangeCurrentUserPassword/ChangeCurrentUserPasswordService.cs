using Krypton.Budgets.Blazor.APIClient._Base;
using Krypton.Budgets.Blazor.APIClient._Impl;

namespace Krypton.Budgets.Blazor.APIClient.Global.ChangeCurrentUserPassword;

public class ChangeCurrentUserPasswordService : PostServiceBase<EmptyResults>
{
    private static readonly string URL = "global/ChangeCurrentUserPassword";

    public ChangeCurrentUserPasswordService(HttpClient client) : base(client, URL) { }

    public async Task ChangeCurrentUserPasswordAsync() => await PostAsync();
}

using Krypton.Budgets.Blazor.APIClient._Base;
using Krypton.Budgets.Blazor.APIClient._Common;

namespace Krypton.Budgets.Blazor.APIClient.Global.AllUsers;

public class AllUsersService : GetServiceBase<AllUsersItem>
{
    private static readonly string URL = "global/AllUsers";

    public AllUsersService(HttpClient client) : base(client, URL) { }

    public async Task<SafeResult<IEnumerable<AllUsersItem?>>> AllUsersAsync() =>
        await GetAsync();
}

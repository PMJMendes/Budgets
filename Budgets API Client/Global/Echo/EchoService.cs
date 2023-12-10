using Krypton.Budgets.Blazor.APIClient._Base;
using Krypton.Budgets.Blazor.APIClient._Common;

namespace Krypton.Budgets.Blazor.APIClient.Global.Echo;

public class EchoService : GetServiceBase<EchoItem>
{
    private static readonly string URL = "global/Echo";

    public EchoService(HttpClient client) : base(client, URL) { }

    public async Task<SafeResult<IEnumerable<EchoItem?>>> EchoAsync() =>
        await GetAsync();
}

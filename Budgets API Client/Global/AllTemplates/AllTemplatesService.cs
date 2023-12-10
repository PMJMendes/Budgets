using Krypton.Budgets.Blazor.APIClient._Base;
using Krypton.Budgets.Blazor.APIClient._Common;

namespace Krypton.Budgets.Blazor.APIClient.Global.AllTemplates;

public class AllTemplatesService : GetServiceBase<AllTemplatesItem>
{
    private static readonly string URL = "global/AllTemplates";

    public AllTemplatesService(HttpClient client) : base(client, URL) { }

    public async Task<SafeResult<IEnumerable<AllTemplatesItem?>>> AllTemplatesAsync() =>
        await GetAsync();
}

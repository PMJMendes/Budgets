using Krypton.Budgets.Blazor.APIClient._Base;
using Krypton.Budgets.Blazor.APIClient._Common;

namespace Krypton.Budgets.Blazor.APIClient.Budgets.Budget.ProductionDetails;

public class ProductionDetailsService : GetServiceBase<TargetQueryArgs, ProductionDetailsItem>
{
    private static readonly string URL = "budgets/ProductionDetails";

    public ProductionDetailsService(HttpClient client) : base(client, URL) { }

    public async Task<SafeResult<IEnumerable<ProductionDetailsItem?>>> ProductionDetailsAsync(TargetQueryArgs args) =>
        await GetAsync(args);
}

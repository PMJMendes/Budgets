using Krypton.Budgets.API._Base;
using Krypton.Budgets.API._Impl;
using Krypton.Budgets.Domain.Budgets.Budget_.ProductionDetails;

namespace Krypton.Budgets.API.Budgets.Budget.ProductionDetails;

internal class ProductionDetails : GetEndpointBase<ProductionDetails, ProductionDetailsItem, TargetQueryArgs>
{
    private readonly IProductionDetails query;

    public ProductionDetails(IProductionDetails query, IHttpContextAccessor accessor, ILogger<ProductionDetails> logger) : base(accessor, logger)
    {
        this.query = query;
    }

    protected override Task<IResult> GetAsync(TargetQueryArgs args) => InRequestScope(() =>
        CallDomainQueryAsync(query, args, r => new ProductionDetailsItem(r)));
}

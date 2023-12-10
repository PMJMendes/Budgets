using Krypton.Budgets.API._Base;
using Krypton.Budgets.API._Impl;
using Krypton.Budgets.API.Budgets.Budget.ProductionDetails;
using Krypton.Budgets.Domain.Budgets.Budget_.ManageBudget;
using Krypton.Budgets.Domain.Budgets.Budget_.ProductionDetails;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Krypton.Budgets.API.Budgets.Budget.ManageBudget;

internal class ManageBudget : PostEndpointBase<ManageBudget, ProductionDetailsItem, ManageBudgetArgs>
{
    private readonly IManageBudget opManage;
    private readonly IProductionDetails query;

    public ManageBudget(IManageBudget opManage, IProductionDetails query,
        IHttpContextAccessor accessor, ILogger<ManageBudget> logger) : base(accessor, logger)
    {
        this.opManage = opManage;
        this.query = query;
    }

    protected override async Task<IResult> PostAsync(ManageBudgetArgs? args) => await InRequestScope(async () =>
    {
        var iResult = await CallDomainOpAsync(opManage, args, _ => EmptyResults.Instance);
        if (iResult is not Ok<EmptyResults>)
        {
            return iResult;
        }

        var queryArgs = new TargetArgs() { TargetId = args?.TargetId };
        var result = await CallDomainQueryAsync(query, queryArgs, r => new ProductionDetailsItem(r));

        if (result is Ok<List<ProductionDetailsItem>> okQResult && okQResult.Value?.FirstOrDefault() is ProductionDetailsItem qResult)
        {
            return Results.Ok(qResult);
        }

        return result;
    });
}

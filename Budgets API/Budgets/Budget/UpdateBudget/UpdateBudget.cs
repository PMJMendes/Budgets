using Krypton.Budgets.API._Base;
using Krypton.Budgets.API._Impl;
using Krypton.Budgets.API.Budgets.Budget.BudgetDetails;
using Krypton.Budgets.Domain.Budgets.Budget_.BudgetDetails;
using Krypton.Budgets.Domain.Budgets.Budget_.RecodeBudget;
using Krypton.Budgets.Domain.Budgets.Budget_.UpdateBudget;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Krypton.Budgets.API.Budgets.Budget.UpdateBudget;

internal class UpdateBudget : PostEndpointBase<UpdateBudget, BudgetDetailsItem, UpdateBudgetArgs>
{
    private readonly IRecodeBudget opRecode;
    private readonly IUpdateBudget opUpdate;
    private readonly IBudgetDetails query;

    public UpdateBudget(IRecodeBudget opRecode, IUpdateBudget opUpdate, IBudgetDetails query,
        IHttpContextAccessor accessor, ILogger<UpdateBudget> logger) : base(accessor, logger)
    {
        this.opRecode = opRecode;
        this.opUpdate = opUpdate;
        this.query = query;
    }

    protected override async Task<IResult> PostAsync(UpdateBudgetArgs? args) => await InRequestScope(async () =>
    {
        var iResult = await CallDomainOpAsync(opRecode, args, _ => EmptyResults.Instance);
        if (iResult is not Ok<EmptyResults>)
        {
            return iResult;
        }

        iResult = await CallDomainOpAsync(opUpdate, args, _ => EmptyResults.Instance);
        if (iResult is not Ok<EmptyResults>)
        {
            return iResult;
        }

        var queryArgs = new TargetArgs() { TargetId = args?.TargetId };
        var result = await CallDomainQueryAsync(query, queryArgs, r => new BudgetDetailsItem(r));

        if (result is Ok<List<BudgetDetailsItem>> okQResult && okQResult.Value?.FirstOrDefault() is BudgetDetailsItem qResult)
        {
            return Results.Ok(qResult);
        }

        return result;
    });
}

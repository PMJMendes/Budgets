using Krypton.Budgets.API._Base;
using Krypton.Budgets.API._Impl;
using Krypton.Budgets.API.Budgets.Budget.BudgetDetails;
using Krypton.Budgets.Domain.Budgets.Budget_.BudgetDetails;
using Krypton.Budgets.Domain.Budgets.Budget_.CloneBudget;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Krypton.Budgets.API.Budgets.Budget.CloneBudget;

internal class CloneBudget : PostEndpointBase<CloneBudget, BudgetDetailsItem, CloneBudgetArgs>
{
    private readonly ICloneBudget op;
    private readonly IBudgetDetails query;

    public CloneBudget(ICloneBudget op, IBudgetDetails query,
        IHttpContextAccessor accessor, ILogger<CloneBudget> logger) : base(accessor, logger)
    {
        this.op = op;
        this.query = query;
    }

    protected override async Task<IResult> PostAsync(CloneBudgetArgs? args) => await InRequestScope(async () =>
    {
        var iResult = await CallDomainOpAsync(op, args, r => new CloneBudgetResults(r));

        if (iResult is Ok<CloneBudgetResults> okOpResult && okOpResult.Value is CloneBudgetResults opResult)
        {
            var queryArgs = new TargetArgs() { TargetId = opResult.NewBudgetId };
            iResult = await CallDomainQueryAsync(query, queryArgs, r => new BudgetDetailsItem(r));
        }

        if (iResult is Ok<List<BudgetDetailsItem>> okQResult && okQResult.Value?.FirstOrDefault() is BudgetDetailsItem qResult)
        {
            return Results.Ok(qResult);
        }

        return iResult;
    });
}

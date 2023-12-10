using Krypton.Budgets.API._Base;
using Krypton.Budgets.API._Impl;
using Krypton.Budgets.API.Budgets.Budget.BudgetDetails;
using Krypton.Budgets.Domain.Budgets.Budget_.BudgetDetails;
using Krypton.Budgets.Domain.Global.CreateBudget;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Krypton.Budgets.API.Global.CreateBudget;

internal class CreateBudget : PostEndpointBase<CreateBudget, CreateBudgetResults, CreateBudgetArgs>
{
    private readonly ICreateBudget op;
    private readonly IBudgetDetails query;
    public CreateBudget(ICreateBudget op, IBudgetDetails query,
        IHttpContextAccessor accessor, ILogger<CreateBudget> logger) : base(accessor, logger)
    {
        this.op = op;
        this.query = query;
    }

    protected override async Task<IResult> PostAsync(CreateBudgetArgs? args) => await InRequestScope(async () =>
    {
        var iResult = await CallDomainOpAsync(op, args, r => new CreateBudgetResults(r));

        if (iResult is Ok<CreateBudgetResults> okOpResult && okOpResult.Value is CreateBudgetResults opResult)
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

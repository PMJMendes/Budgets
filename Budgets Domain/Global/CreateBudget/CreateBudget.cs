using Krypton.Budgets.Domain._Base.Services;
using Krypton.Budgets.Domain._Impl.Interfaces;
using Krypton.Budgets.Domain.Budgets.Budget_;
using Microsoft.Extensions.Logging;

namespace Krypton.Budgets.Domain.Global.CreateBudget;

internal class CreateBudget : BaseOperation<CreateBudget, ICreateBudgetArgs, ICreateBudgetResults>, ICreateBudget
{
    public CreateBudget(IContext context, ILogger<CreateBudget> logger) : base(context, logger) { }

    protected override void AssertPermissions() => AssertIsProducer();

    protected override async Task<V> InnerExecute<V>(ICreateBudgetArgs? args, Func<ICreateBudgetResults, V> resultsFactory)
    {
        var code = args?.Code?.Trim() ?? throw BuildUnexpectedValidationFailure();
        var asTemplate = args?.AsTemplate ?? throw BuildUnexpectedValidationFailure();
        var budgetDate = args?.BudgetDate ?? throw BuildUnexpectedValidationFailure();
        var title = args?.Title?.Trim() ?? throw BuildUnexpectedValidationFailure();

        var newBudget = context.GlobalObject.CreateBudget(code, asTemplate, budgetDate, title);

        await context.Persistence.FlushChangesAsync();

        return resultsFactory(new Result(
            newBudget.Id,
            newBudget.IsTemplate,
            newBudget.State
        ));
    }

    private record struct Result(
        Guid NewBudgetId,
        bool IsTemplate,
        BudgetState State
    ) : ICreateBudgetResults;
}

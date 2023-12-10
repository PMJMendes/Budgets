using Krypton.Budgets.Domain._Base.Services;
using Krypton.Budgets.Domain._Impl.Interfaces;
using Microsoft.Extensions.Logging;

namespace Krypton.Budgets.Domain.Budgets.Budget_.CloneBudget;

internal class CloneBudget : BaseOperation<CloneBudget, ICloneBudgetArgs, ICloneBudgetResults>, ICloneBudget
{
    public CloneBudget(IContext context, ILogger<CloneBudget> logger) : base(context, logger) { }

    protected override void AssertPermissions() => AssertIsProducer();

    protected override async Task<V> InnerExecute<V>(ICloneBudgetArgs? args, Func<ICloneBudgetResults, V> resultsFactory)
    {
        var newCode = args?.NewCode?.Trim() ?? throw BuildUnexpectedValidationFailure();
        var asTemplate = args?.AsTemplate ?? throw BuildUnexpectedValidationFailure();
        var newBudgetDate = args?.NewBudgetDate ?? throw BuildUnexpectedValidationFailure();
        var newTitle = args?.NewTitle;

        var budget = await GetTarget<Budget>(args);

        var newBudget = await budget.CloneAsync(newCode, asTemplate, newBudgetDate, newTitle);

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
    ) : ICloneBudgetResults;
}

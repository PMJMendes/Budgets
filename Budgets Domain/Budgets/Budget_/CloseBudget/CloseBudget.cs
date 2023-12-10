using Krypton.Budgets.Domain._Base.Interfaces;
using Krypton.Budgets.Domain._Base.Services;
using Krypton.Budgets.Domain._Impl.Interfaces;
using Microsoft.Extensions.Logging;

namespace Krypton.Budgets.Domain.Budgets.Budget_.CloseBudget;

internal class CloseBudget : BaseOperation<CloseBudget, ITargetArgs, ICloseBudgetResults>, ICloseBudget
{
    public CloseBudget(IContext context, ILogger<CloseBudget> logger) : base(context, logger) { }

    protected override void AssertPermissions() => AssertIsProducer();

    protected override async Task<V> InnerExecute<V>(ITargetArgs? args, Func<ICloseBudgetResults, V> resultsFactory)
    {
        var budget = await GetTarget<Budget>(args);

        budget.Close();

        await context.Persistence.FlushChangesAsync();

        return resultsFactory(new Result(
            budget.State
        ));
    }

    private record struct Result(
        BudgetState State
    ) : ICloseBudgetResults;
}

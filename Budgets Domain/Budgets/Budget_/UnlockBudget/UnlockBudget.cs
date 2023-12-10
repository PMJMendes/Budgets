using Krypton.Budgets.Domain._Base.Interfaces;
using Krypton.Budgets.Domain._Base.Services;
using Krypton.Budgets.Domain._Impl.Interfaces;
using Microsoft.Extensions.Logging;

namespace Krypton.Budgets.Domain.Budgets.Budget_.UnlockBudget;

internal class UnlockBudget : BaseOperation<UnlockBudget, ITargetArgs, IUnlockBudgetResults>, IUnlockBudget
{
    public UnlockBudget(IContext context, ILogger<UnlockBudget> logger) : base(context, logger) { }

    protected override void AssertPermissions() => AssertIsProducer();

    protected override async Task<V> InnerExecute<V>(ITargetArgs? args, Func<IUnlockBudgetResults, V> resultsFactory)
    {
        var budget = await GetTarget<Budget>(args);

        budget.Unlock();

        await context.Persistence.FlushChangesAsync();

        return resultsFactory(new Result(
            budget.State
        ));
    }

    private record struct Result(
        BudgetState State
    ) : IUnlockBudgetResults;
}

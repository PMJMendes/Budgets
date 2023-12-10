using Krypton.Budgets.Domain._Base.Interfaces;
using Krypton.Budgets.Domain._Base.Services;
using Krypton.Budgets.Domain._Impl.Interfaces;
using Microsoft.Extensions.Logging;

namespace Krypton.Budgets.Domain.Budgets.Budget_.LockBudget;

internal class LockBudget : BaseOperation<LockBudget, ITargetArgs, ILockBudgetResults>, ILockBudget
{
    public LockBudget(IContext context, ILogger<LockBudget> logger) : base(context, logger) { }

    protected override void AssertPermissions() => AssertIsProducer();

    protected override async Task<V> InnerExecute<V>(ITargetArgs? args, Func<ILockBudgetResults, V> resultsFactory)
    {
        var budget = await GetTarget<Budget>(args);

        budget.Lock();

        await context.Persistence.FlushChangesAsync();

        return resultsFactory(new Result(
            budget.State
        ));
    }

    private record struct Result(
        BudgetState State
    ) : ILockBudgetResults;
}

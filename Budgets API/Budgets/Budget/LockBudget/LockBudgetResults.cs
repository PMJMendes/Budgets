using Krypton.Budgets.Domain.Budgets.Budget_;
using Krypton.Budgets.Domain.Budgets.Budget_.LockBudget;

namespace Krypton.Budgets.API.Budgets.Budget.LockBudget;

internal readonly struct LockBudgetResults : ILockBudgetResults
{
    public LockBudgetResults(ILockBudgetResults source)
    {
        State = source.State;
    }

    public BudgetState State { get; private init; }
}

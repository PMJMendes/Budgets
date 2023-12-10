using Krypton.Budgets.Domain.Budgets.Budget_;
using Krypton.Budgets.Domain.Budgets.Budget_.UnlockBudget;

namespace Krypton.Budgets.API.Budgets.Budget.UnlockBudget;

internal readonly struct UnlockBudgetResults : IUnlockBudgetResults
{
    public UnlockBudgetResults(IUnlockBudgetResults source)
    {
        State = source.State;
    }

    public BudgetState State { get; private init; }
}

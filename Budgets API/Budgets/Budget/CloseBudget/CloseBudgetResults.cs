using Krypton.Budgets.Domain.Budgets.Budget_;
using Krypton.Budgets.Domain.Budgets.Budget_.CloseBudget;

namespace Krypton.Budgets.API.Budgets.Budget.CloseBudget;

internal readonly struct CloseBudgetResults : ICloseBudgetResults
{
    public CloseBudgetResults(ICloseBudgetResults source)
    {
        State = source.State;
    }

    public BudgetState State { get; private init; }
}

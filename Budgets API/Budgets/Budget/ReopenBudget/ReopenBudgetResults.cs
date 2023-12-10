using Krypton.Budgets.Domain.Budgets.Budget_;
using Krypton.Budgets.Domain.Budgets.Budget_.ReopenBudget;

namespace Krypton.Budgets.API.Budgets.Budget.ReopenBudget;

internal readonly struct ReopenBudgetResults : IReopenBudgetResults
{
    public ReopenBudgetResults(IReopenBudgetResults source)
    {
        State = source.State;
    }

    public BudgetState State { get; private init; }
}

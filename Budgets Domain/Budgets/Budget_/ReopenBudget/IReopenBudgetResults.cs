using Krypton.Budgets.Domain._Base.Interfaces;

namespace Krypton.Budgets.Domain.Budgets.Budget_.ReopenBudget;

public interface IReopenBudgetResults : IOpResults
{
    BudgetState State { get; }
}

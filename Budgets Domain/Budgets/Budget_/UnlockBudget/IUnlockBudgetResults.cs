using Krypton.Budgets.Domain._Base.Interfaces;

namespace Krypton.Budgets.Domain.Budgets.Budget_.UnlockBudget;

public interface IUnlockBudgetResults : IOpResults
{
    BudgetState State { get; }
}

using Krypton.Budgets.Domain._Base.Interfaces;

namespace Krypton.Budgets.Domain.Budgets.Budget_.CloseBudget;

public interface ICloseBudgetResults : IOpResults
{
    BudgetState State { get; }
}

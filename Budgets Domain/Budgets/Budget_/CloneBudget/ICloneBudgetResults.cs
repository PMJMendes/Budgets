using Krypton.Budgets.Domain._Base.Interfaces;

namespace Krypton.Budgets.Domain.Budgets.Budget_.CloneBudget;

public interface ICloneBudgetResults : IOpResults
{
    Guid NewBudgetId { get; }

    bool IsTemplate { get; }

    BudgetState State { get; }
}

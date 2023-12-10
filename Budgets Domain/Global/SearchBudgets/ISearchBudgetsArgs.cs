using Krypton.Budgets.Domain._Base.Interfaces;
using Krypton.Budgets.Domain.Budgets.Budget_;

namespace Krypton.Budgets.Domain.Global.SearchBudgets;

public interface ISearchBudgetsArgs : IArguments
{
    string? FreeText { get; }

    BudgetState? State { get; }

    DateOnly? BudgetDateFrom { get; }
    DateOnly? BudgetDateTo { get; }
}

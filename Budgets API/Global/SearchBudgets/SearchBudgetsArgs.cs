using Krypton.Budgets.Domain.Budgets.Budget_;
using Krypton.Budgets.Domain.Global.SearchBudgets;

namespace Krypton.Budgets.API.Global.SearchBudgets;

internal struct SearchBudgetsArgs : ISearchBudgetsArgs
{
    public string? FreeText { get; set; }

    public BudgetState? State { get; set; }

    public DateOnly? BudgetDateFrom { get; set; }

    public DateOnly? BudgetDateTo { get; set; }
}

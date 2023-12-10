using Krypton.Budgets.Blazor.APIClient._Base;

namespace Krypton.Budgets.Blazor.APIClient.Global.SearchBudgets;

public class SearchBudgetsArgs : IQueryArgs
{
    public SearchBudgetsArgs(string? freeText, string? state, DateOnly? budgetDateFrom, DateOnly? budgetDateTo)
    {
        FreeText = freeText;
        State = state;
        BudgetDateFrom = budgetDateFrom;
        BudgetDateTo = budgetDateTo;
    }

    public string? FreeText { get; private init; }
    public string? State { get; private init; }
    public DateOnly? BudgetDateFrom { get; private init; }
    public DateOnly? BudgetDateTo { get; private init; }

    public Dictionary<string, string?> GetQueryString() => new()
    {
        [nameof(FreeText)] = FreeText,
        [nameof(State)] = State,
        [nameof(BudgetDateFrom)] = BudgetDateFrom?.ToString("yyyy-MM-dd"),
        [nameof(BudgetDateTo)] = BudgetDateTo?.ToString("yyyy-MM-dd")
    };
}

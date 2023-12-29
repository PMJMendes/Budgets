using Krypton.Budgets.Blazor.APIClient.Global.SearchBudgets;

namespace Krypton.Budgets.Blazor.UI.Budgets._Common;

public class BudgetsSearchModel
{
    private BudgetsSearchModel(string? text, BudgetState state, DateTime? budgetDateFrom, DateTime? budgetDateTo)
    {
        Text = text;
        State = state;
        BudgetDateFrom = budgetDateFrom;
        BudgetDateTo = budgetDateTo;
    }

    public string? Text { get; set; }
    public BudgetState State { get; set; }
    public DateTime? BudgetDateFrom { get; set; }
    public DateTime? BudgetDateTo { get; set; }

    public SearchBudgetsArgs AsArgs() => new(
        Text,
        State == BudgetState._UNKNOWN ? null : State.ToString(),
        BudgetDateFrom is DateTime from ? DateOnly.FromDateTime(from) : null,
        BudgetDateFrom is DateTime to ? DateOnly.FromDateTime(to) : null

    );

    public static BudgetsSearchModel Empty() => new(
        null,
        BudgetState._UNKNOWN,
        null,
        null
    );
}

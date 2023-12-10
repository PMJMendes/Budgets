using Krypton.Budgets.Blazor.APIClient.Global.SearchBudgets;
using Krypton.Budgets.Blazor.UI.Users._Common;

namespace Krypton.Budgets.Blazor.UI.Budgets._Common;

public class BudgetsSearchModel
{
    private BudgetsSearchModel(string? text, UserState state, DateTime? budgetDateFrom, DateTime? budgetDateTo)
    {
        Text = text;
        State = state;
        BudgetDateFrom = budgetDateFrom;
        BudgetDateTo = budgetDateTo;
    }

    public string? Text { get; set; }
    public UserState State { get; set; }
    public DateTime? BudgetDateFrom { get; set; }
    public DateTime? BudgetDateTo { get; set; }

    public SearchBudgetsArgs AsArgs() => new(
        Text,
        State == UserState._UNKNOWN ? null : State.ToString(),
        BudgetDateFrom is DateTime from ? DateOnly.FromDateTime(from) : null,
        BudgetDateFrom is DateTime to ? DateOnly.FromDateTime(to) : null

    );

    public static BudgetsSearchModel Empty() => new(
        null,
        UserState._UNKNOWN,
        null,
        null
    );
}

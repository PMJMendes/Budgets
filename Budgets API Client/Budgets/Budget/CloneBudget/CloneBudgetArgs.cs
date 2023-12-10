using Krypton.Budgets.Blazor.APIClient._Common;

namespace Krypton.Budgets.Blazor.APIClient.Budgets.Budget.CloneBudget;

public class CloneBudgetArgs : TargetPostArgs
{
    public CloneBudgetArgs(Guid targetId, string newCode, bool asTemplate, DateOnly newBudgetDate, string? newTitle) : base(targetId)
    {
        NewCode = newCode;
        AsTemplate = asTemplate;
        NewBudgetDate = newBudgetDate;
        NewTitle = newTitle;
    }

    public string NewCode { get; private init; }
    public bool AsTemplate { get; private init; }
    public DateOnly NewBudgetDate { get; private init; }
    public string? NewTitle { get; private init; }
}

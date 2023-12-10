namespace Krypton.Budgets.Blazor.APIClient.Global.CreateBudget;

public class CreateBudgetArgs
{
    public CreateBudgetArgs(string code, bool asTemplate, DateOnly budgetDate, string title)
    {
        Code = code;
        AsTemplate = asTemplate;
        BudgetDate = budgetDate;
        Title = title;
    }

    public string Code { get; private init; }
    public bool AsTemplate { get; private init; }
    public DateOnly BudgetDate { get; private init; }
    public string Title { get; private init; }
}

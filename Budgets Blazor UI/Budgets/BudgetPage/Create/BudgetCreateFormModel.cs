using Krypton.Budgets.Blazor.APIClient.Budgets.Budget.CloneBudget;
using Krypton.Budgets.Blazor.APIClient.Global.CreateBudget;
using Krypton.Budgets.Blazor.UI._Shared;
using Krypton.Budgets.Blazor.UI.Budgets.BudgetPage.View;

namespace Krypton.Budgets.Blazor.UI.Budgets.BudgetPage.Create;

public class BudgetCreateFormModel
{
    private BudgetCreateFormModel(string code, RefModel template, DateTime budgetDate, string title, bool fromExisting)
    {
        Code = code;
        Template = template;
        BudgetDate = budgetDate;
        Title = title;
        FromExisting = fromExisting;
    }

    public string Code { get; set; }
    public RefModel Template { get; set; }
    public DateTime? BudgetDate { get; set; } // Never null. Must be nullable because of DatePicker.
    public string Title { get; set; }
    public bool FromExisting { get; private init; }

    public CreateBudgetArgs AsCreateArgs() => new(
        Code,
        false,
        DateOnly.FromDateTime((DateTime)BudgetDate!),
        Title
    );

    public CloneBudgetArgs AsCloneArgs() => new(
        Template.Id,
        Code,
        false,
        DateOnly.FromDateTime((DateTime)BudgetDate!),
        Title
    );

    public BudgetCreateFormModel WithCode(string code) => new(
        code,
        Template,
        (DateTime)BudgetDate!,
        Title,
        FromExisting
    );

    public static BudgetCreateFormModel Empty() => new(
        string.Empty,
        RefModel.Empty(),
        DateTime.Today,
        string.Empty,
        false
    );

    public static BudgetCreateFormModel FromSource(BudgetViewModel source) => new(
        string.Empty,
        RefModel.FromBudgetSource(source),
        DateTime.Today,
        source.BudgetData.FrontData.Title + " dup.",
        true
    );
}

using Krypton.Budgets.Blazor.APIClient.Budgets.Budget.ManageBudget;
using Krypton.Budgets.Blazor.APIClient.Budgets.Budget.UpdateBudget;
using Krypton.Budgets.Blazor.UI.Budgets.BudgetPage._Core;
using Krypton.Budgets.Blazor.UI.Budgets.BudgetPage.View;

namespace Krypton.Budgets.Blazor.UI.Budgets.BudgetPage.Manage;

public class BudgetManageModel
{
    private BudgetManageModel(Guid id, string code, BudgetModel budgetData)
    {
        Id = id;
        Code = code;
        BudgetData = budgetData;
    }

    public Guid Id { get; private init; }
    public string Code { get; private init; }
    public BudgetModel BudgetData { get; private init; }

    public UpdateBudgetArgs AsUpdateArgs() => new(
        Id,
        Code,
        BudgetData.FrontData.AsArgs(),
        BudgetData.PercentsData.AsArgs(),
        BudgetData.Groups.Select(g => g.AsUpdateArgs()).ToList()
    );

    public ManageBudgetArgs AsManageArgs() => new(
        Id,
        BudgetData.PercentsData.NWeatherDays,
        BudgetData.Groups.Select(g => g.AsManageArgs()).ToList()
    );

    public static BudgetManageModel Empty() => new(
        Guid.Empty,
        string.Empty,
        BudgetModel.Empty()
    );

    public static BudgetManageModel FromViewData(BudgetViewModel viewData) => new(
        viewData.Id,
        viewData.Code,
        viewData.BudgetData.Clone()
    );
}

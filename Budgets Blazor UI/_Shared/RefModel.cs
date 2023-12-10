using Krypton.Budgets.Blazor.APIClient._Impl;
using Krypton.Budgets.Blazor.APIClient.Global.AllTemplates;
using Krypton.Budgets.Blazor.APIClient.Global.AllUsers;
using Krypton.Budgets.Blazor.UI.Budgets.BudgetPage.View;
using Krypton.Budgets.Blazor.UI.Templates.TemplatePage.View;

namespace Krypton.Budgets.Blazor.UI._Shared;

public class RefModel
{
    private RefModel(Guid id, string description)
    {
        Id = id;
        Description = description;
    }

    public Guid Id { get; private init; }
    public string Description { get; private init; }

    public static RefModel Empty() => new(
        Guid.Empty,
        string.Empty
    );

    public static RefModel FromRefItem(RefItem item) => new(
        item.Id ?? Guid.Empty,
        item.Description ?? string.Empty
    );

    public static RefModel FromAllTemplates(AllTemplatesItem item) => new(
        item.Id ?? Guid.Empty,
        item.Title ?? string.Empty
    );

    public static RefModel FromAllUsers(AllUsersItem item) => new(
        item.Id ?? Guid.Empty,
        item.FullName ?? string.Empty
    );

    public static RefModel FromBudgetSource(BudgetViewModel source) => new(
        source.Id,
        source.BudgetData.FrontData.Title ?? string.Empty
    );

    public static RefModel FromTemplateSource(TemplateViewModel source) => new(
        source.Id,
        source.TemplateData.FrontData.Title ?? string.Empty
    );
}

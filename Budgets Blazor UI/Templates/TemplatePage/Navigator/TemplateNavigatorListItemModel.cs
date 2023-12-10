using Krypton.Budgets.Blazor.APIClient.Budgets.Budget.BudgetDetails;
using Krypton.Budgets.Blazor.APIClient.Global.AllTemplates;
using Krypton.Budgets.Blazor.UI.Templates.TemplatesRoot;

namespace Krypton.Budgets.Blazor.UI.Templates.TemplatePage.Navigator;

public class TemplateNavigatorListItemModel
{
    public TemplateNavigatorListItemModel(Guid id, string code, string title)
    {
        Id = id;
        Code = code;
        Title = title;
    }

    public Guid Id { get; private init; }
    public string Code { get; private init; }
    public string Title { get; private init; }

    public static TemplateNavigatorListItemModel Empty() => new(
        Guid.Empty,
        "",
        ""
    );

    public static TemplateNavigatorListItemModel FromRoot(TemplatesRootListItemModel rootModel) => new(
        rootModel.Id,
        rootModel.Code,
        rootModel.Title
    );

    public static TemplateNavigatorListItemModel FromService(AllTemplatesItem item) => new(
        item.Id ?? Guid.Empty,
        item.Code ?? "",
        item.Title ?? ""
    );

    public static TemplateNavigatorListItemModel FromCreateResults(BudgetDetailsItem? results) => new(
        results?.Id ?? Guid.Empty,
        results?.Code ?? "",
        results?.Title ?? ""
    );

    public static TemplateNavigatorListItemModel FromEditResults(BudgetDetailsItem? results) => new(
        results?.Id ?? Guid.Empty,
        results?.Code ?? "",
        results?.Title ?? ""
    );
}

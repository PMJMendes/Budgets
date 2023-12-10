using Krypton.Budgets.Blazor.APIClient.Budgets.Budget.BudgetDetails;
using Krypton.Budgets.Blazor.APIClient.Global.AllTemplates;
using Krypton.Budgets.Blazor.UI.Templates.TemplatesRoot;

namespace Krypton.Budgets.Blazor.UI.Templates.TemplatePage.Navigator;

public class TemplateNavigatorModel
{
    private TemplateNavigatorModel(
        IEnumerable<TemplateNavigatorListItemModel> templates,
        Guid selectedId)
    {
        Templates = templates;
        SelectedId = selectedId;
    }

    public IEnumerable<TemplateNavigatorListItemModel> Templates { get; private init; }
    public Guid SelectedId { get; private init; }

    public TemplateNavigatorModel WithId(Guid id) => new(
        Templates,
        id
    );

    public TemplateNavigatorModel WithTemplates(IEnumerable<AllTemplatesItem?>? items) => new(
        (items ?? Array.Empty<AllTemplatesItem>()).
            Where(i => i is not null).
            Select(i => TemplateNavigatorListItemModel.FromService(i!)).
            ToList(),
        SelectedId
    );

    public TemplateNavigatorModel WithCreateResults(BudgetDetailsItem? results) => new(
        Templates.Prepend(TemplateNavigatorListItemModel.FromCreateResults(results)).ToList(),
        results?.Id ?? Guid.Empty
    );

    public TemplateNavigatorModel WithEditResults(BudgetDetailsItem? results) => new(
        Templates.
            Select(c => c.Id == (results?.Id ?? Guid.Empty) ? TemplateNavigatorListItemModel.FromEditResults(results) : c).
            ToList(),
        results?.Id ?? Guid.Empty
    );

    public static TemplateNavigatorModel Empty() => new(
        Array.Empty<TemplateNavigatorListItemModel>(),
        Guid.Empty
    );

    public static TemplateNavigatorModel FromRoot(TemplatesRootModel rootModel) => new(
        rootModel.Items.
            Select(i => TemplateNavigatorListItemModel.FromRoot(i)).
            ToList(),
        Guid.Empty
    );
}

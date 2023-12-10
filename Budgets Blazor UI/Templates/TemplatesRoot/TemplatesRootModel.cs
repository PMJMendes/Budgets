using Krypton.Budgets.Blazor.APIClient.Global.AllTemplates;

namespace Krypton.Budgets.Blazor.UI.Templates.TemplatesRoot;

public class TemplatesRootModel
{
    private TemplatesRootModel(
        IEnumerable<TemplatesRootListItemModel> items)
    {
        Items = items;
    }

    public IEnumerable<TemplatesRootListItemModel> Items { get; private init; }

    public static TemplatesRootModel Empty() => new(
        Array.Empty<TemplatesRootListItemModel>()
    );

    public static TemplatesRootModel WithResults(IEnumerable<AllTemplatesItem?>? items) => new(
        (items ?? Array.Empty<AllTemplatesItem>()).
            Where(i => i is not null).
            Select(i => TemplatesRootListItemModel.FromService(i!)).
            ToList()
    );
}

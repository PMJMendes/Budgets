using Krypton.Budgets.Blazor.APIClient.Global.AllTemplates;

namespace Krypton.Budgets.Blazor.UI.Templates.TemplatesRoot;

public class TemplatesRootListItemModel
{
    private TemplatesRootListItemModel(
        Guid id,
        string code,
        string title)
    {
        Id = id;
        Code = code;
        Title = title;
    }

    public Guid Id { get; private init; }
    public string Code { get; private init; }
    public string Title { get; private init; }

    public static TemplatesRootListItemModel Empty() => new(
        Guid.Empty,
        "",
        ""
    );

    public static TemplatesRootListItemModel FromService(AllTemplatesItem item)
    {
        return new(
            item.Id ?? Guid.Empty,
            item.Code ?? "",
            item.Title ?? ""
        );
    }
}

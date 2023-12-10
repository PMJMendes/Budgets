using Krypton.Budgets.Domain.Global.AllTemplates;

namespace Krypton.Budgets.API.Global.AllTemplates;

internal readonly struct AllTemplatesItem : IAllTemplatesItem
{
    public AllTemplatesItem(IAllTemplatesItem source)
    {
        Id = source.Id;
        Code = source.Code;
        Title = source.Title;
    }

    public Guid Id { get; private init; }
    public string Code { get; private init; }
    public string Title { get; private init; }
}

using Krypton.Budgets.Domain._Base.Interfaces;

namespace Krypton.Budgets.Domain.Global.AllTemplates;

public interface IAllTemplatesItem : IQueryResultItem
{
    Guid Id { get; }

    string Code { get; }

    string Title { get; }
}

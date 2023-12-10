namespace Krypton.Budgets.Domain._Base.Interfaces;

public interface IRefItem : IQueryResultItem
{
    public Guid Id { get; }
    public string Description { get; }
}

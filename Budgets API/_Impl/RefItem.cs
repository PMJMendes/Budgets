using Krypton.Budgets.Domain._Base.Interfaces;

namespace Krypton.Budgets.API._Impl;

internal class RefItem : IRefItem
{
    public RefItem(IRefItem source)
    {
        Id = source.Id;
        Description = source.Description;
    }

    public Guid Id { get; private init; }
    public string? Description { get; private init; }
}

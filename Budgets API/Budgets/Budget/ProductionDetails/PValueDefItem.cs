using Krypton.Budgets.Domain.Budgets.Budget_.ProductionDetails;
using ValueType = Krypton.Budgets.Shared.ValueType;

namespace Krypton.Budgets.API.Budgets.Budget.ProductionDetails;

internal readonly struct PValueDefItem : IPValueDefItem
{
    public PValueDefItem(IPValueDefItem source)
    {
        Id = source.Id;
        Type = source.Type;
        Description = source.Description;
        DescEnglish = source.DescEnglish;
    }

    public Guid Id { get; private init; }
    public ValueType Type { get; private init; }
    public string? Description { get; private init; }
    public string? DescEnglish { get; private init; }
}

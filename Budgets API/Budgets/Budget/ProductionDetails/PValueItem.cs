using Krypton.Budgets.Domain.Budgets.Budget_.ProductionDetails;

namespace Krypton.Budgets.API.Budgets.Budget.ProductionDetails;

internal readonly struct PValueItem : IPValueItem
{
    public PValueItem(IPValueItem source)
    {
        Id = source.Id;
        NumberValue = source.NumberValue;
        TextValue = source.TextValue;
        TextEnglish = source.TextEnglish;
    }

    public Guid Id { get; private init; }
    public decimal? NumberValue { get; private init; }
    public string? TextValue { get; private init; }
    public string? TextEnglish { get; private init; }
}

using Krypton.Budgets.Domain.Budgets.Budget_.BudgetDetails;

namespace Krypton.Budgets.API.Budgets.Budget.BudgetDetails;

internal readonly struct ValueItem : IValueItem
{
    public ValueItem(IValueItem source)
    {
        Id = source.Id;
        NumberValue = source.NumberValue;
        TextValue = source.TextValue;
        TextEnglish = source.TextEnglish;
        ProdValue = source.ProdValue;
    }

    public Guid Id { get; private init; }
    public decimal? NumberValue { get; private init; }
    public string? TextValue { get; private init; }
    public string? TextEnglish { get; private init; }
    public decimal? ProdValue { get; private init; }
}

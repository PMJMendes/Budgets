using Krypton.Budgets.Domain.Budgets.Budget_.BudgetDetails;
using ValueType = Krypton.Budgets.Shared.ValueType;

namespace Krypton.Budgets.API.Budgets.Budget.BudgetDetails;

internal readonly struct ValueDefItem : IValueDefItem
{
    public ValueDefItem(IValueDefItem source)
    {
        Id = source.Id;
        Type = source.Type;
        Description = source.Description;
        DescEnglish = source.DescEnglish;
        BCAFormula = source.BCAFormula;
    }

    public Guid Id { get; private init; }
    public ValueType Type { get; private init; }
    public string? Description { get; private init; }
    public string? DescEnglish { get; private init; }
    public string? BCAFormula { get; private init; }
}

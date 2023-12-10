using ValueType = Krypton.Budgets.Shared.ValueType;

namespace Krypton.Budgets.Domain.Budgets.ValueDef_;

internal record struct ValueDefData(
    Guid Id,
    ValueType Type,
    string Description,
    string? DescEnglish,
    string? BCAFormula
);

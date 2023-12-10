namespace Krypton.Budgets.Domain.Budgets.Cost_;

internal record struct CostData(
    Guid Id,
    decimal CostValue,
    string? Supplier,
    string? Text
);

namespace Krypton.Budgets.Domain.Budgets.Item_;

internal record struct ItemDefData(
    Guid Id,
    bool ExcludeFromBase,
    bool CanBePercent,
    string? Description,
    string? DescEnglish
);

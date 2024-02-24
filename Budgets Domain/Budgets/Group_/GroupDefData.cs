using Krypton.Budgets.Domain.Budgets.Category_;

namespace Krypton.Budgets.Domain.Budgets.Group_;

internal record struct GroupDefData(
    Guid Id,
    string? Description,
    string? DescEnglish,
    IEnumerable<CategoryDefData> CategoryDefData
);

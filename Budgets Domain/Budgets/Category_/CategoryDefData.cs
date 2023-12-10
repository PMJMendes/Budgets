using Krypton.Budgets.Domain.Budgets.Item_;
using Krypton.Budgets.Domain.Budgets.ValueDef_;

namespace Krypton.Budgets.Domain.Budgets.Category_;

internal record struct CategoryDefData(
    Guid Id,
    string Formula,
    string Description,
    string? DescEnglish,
    IEnumerable<ValueDefData> ValueDefData,
    IEnumerable<ItemDefData> ItemDefData
);

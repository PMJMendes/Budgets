using Krypton.Budgets.Domain.Budgets.Item_;

namespace Krypton.Budgets.Domain.Budgets.Category_;

internal record struct CategoryData(
    IEnumerable<ItemData> ItemData);

using Krypton.Budgets.Domain.Budgets.Category_;

namespace Krypton.Budgets.Domain.Budgets.Group_;

internal record struct GroupData(
    IEnumerable<CategoryData> CategoryData);

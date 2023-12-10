using Krypton.Budgets.Domain.Budgets.Group_;

namespace Krypton.Budgets.Domain.Budgets.Budget_;

internal record struct BudgetData(
    IEnumerable<GroupData> GroupData
);

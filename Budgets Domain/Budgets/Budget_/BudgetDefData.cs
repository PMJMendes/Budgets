using Krypton.Budgets.Domain.Budgets.Group_;

namespace Krypton.Budgets.Domain.Budgets.Budget_;

internal record struct BudgetDefData(
    IEnumerable<GroupDefData> GroupDefData
);

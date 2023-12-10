using ValueType = Krypton.Budgets.Shared.ValueType;

namespace Krypton.Budgets.Domain.Budgets.Budget_.BudgetDetails;

public interface IValueDefItem
{
    Guid Id { get; }

    ValueType Type { get; }
    string Description { get; }
    string? DescEnglish { get; }
    string? BCAFormula { get; }
}

using Krypton.Budgets.Domain.Budgets.Budget_.UpdateBudget;
using ValueType = Krypton.Budgets.Shared.ValueType;

namespace Krypton.Budgets.API.Budgets.Budget.UpdateBudget;

internal class ValueDefArgs : IValueDefArgs
{
    public Guid? Id { get; set; }
    public ValueType? Type { get; set; }
    public string? Description { get; set; }
    public string? DescEnglish { get; set; }
    public string? BCAFormula { get; set; }
}

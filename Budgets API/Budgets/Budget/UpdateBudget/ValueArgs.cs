using Krypton.Budgets.Domain.Budgets.Budget_.UpdateBudget;

namespace Krypton.Budgets.API.Budgets.Budget.UpdateBudget;

internal class ValueArgs : IValueArgs
{
    public Guid? Id { get; set; }

    public decimal? NumberValue { get; set; }
    public string? TextValue { get; set; }
    public string? TextEnglish { get; set; }
    public decimal? ProdValue { get; set; }
}

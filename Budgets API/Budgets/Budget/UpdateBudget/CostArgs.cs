using Krypton.Budgets.Domain.Budgets.Budget_.UpdateBudget;

namespace Krypton.Budgets.API.Budgets.Budget.UpdateBudget;

internal class CostArgs : ICostArgs
{
	public Guid? Id { get; set; }

	public decimal? CostValue { get; set; }
	public string? Supplier { get; set; }
	public string? Text { get; set; }
}

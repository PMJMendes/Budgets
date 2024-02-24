using Krypton.Budgets.Domain._Impl.Attributes;
using Krypton.Budgets.Domain.Budgets.Cost_;

namespace Krypton.Budgets.Domain.Budgets.Budget_.UpdateBudget;

public interface ICostArgs
{
	Guid? Id { get; }

	[Required]
	decimal? CostValue { get; }

	string? Supplier { get; }
	string? Text { get; }

	internal sealed CostData ToCostData() => new(
		Id ?? Guid.Empty,
		CostValue ?? 0m,
		Supplier,
		Text
	);
}

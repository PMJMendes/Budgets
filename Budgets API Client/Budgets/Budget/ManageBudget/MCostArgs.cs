namespace Krypton.Budgets.Blazor.APIClient.Budgets.Budget.ManageBudget;

public class MCostArgs
{
	public MCostArgs(Guid? id, decimal costValue, string? supplier, string? text)
	{
		Id = id;
		CostValue = costValue;
		Supplier = supplier;
		Text = text;
	}

	public Guid? Id { get; private init; }
	public decimal CostValue { get; private init; }
	public string? Supplier { get; private init; }
	public string? Text { get; private init; }
}

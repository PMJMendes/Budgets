namespace Krypton.Budgets.Blazor.APIClient.Budgets.Budget.UpdateBudget;

public class ItemArgs
{
	public ItemArgs(Guid id, bool excludeFromBase, bool canBePercent, string description, string? descEnglish,
		decimal? percent, decimal? bCAPercent, IEnumerable<ValueArgs> valueArgs, IEnumerable<CostArgs> costArgs)
	{
		Id = id;
		ExcludeFromBase = excludeFromBase;
		CanBePercent = canBePercent;
		Description = description;
		DescEnglish = descEnglish;

		Percent = percent;
		BCAPercent = bCAPercent;
		ValueArgs = valueArgs;
		CostArgs = costArgs;
	}

	public Guid Id { get; private init; }
	public bool ExcludeFromBase { get; private init; }
	public bool CanBePercent { get; private init; }
	public string Description { get; private init; }
	public string? DescEnglish { get; private init; }

	public decimal? Percent { get; private init; }
	public decimal? BCAPercent { get; private init; }
	public IEnumerable<ValueArgs> ValueArgs { get; private init; }
	public IEnumerable<CostArgs> CostArgs { get; private init; }
}

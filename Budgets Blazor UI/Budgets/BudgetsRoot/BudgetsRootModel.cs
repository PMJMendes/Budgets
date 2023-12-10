using Krypton.Budgets.Blazor.APIClient.Global.SearchBudgets;
using Krypton.Budgets.Blazor.UI.Budgets._Common;

namespace Krypton.Budgets.Blazor.UI.Budgets.BudgetsRoot;

public class BudgetsRootModel
{
	private BudgetsRootModel(
		BudgetsSearchModel searchArgs,
		IEnumerable<BudgetsRootListItemModel> items)
	{
		SearchArgs = searchArgs;
		Items = items;
	}

	public BudgetsSearchModel SearchArgs { get; private init; }
	public IEnumerable<BudgetsRootListItemModel> Items { get; private init; }

	public BudgetsRootModel WithSearchArgs(BudgetsSearchModel searchArgs) => new(
		searchArgs,
		Items
	);

	public BudgetsRootModel WithSearchResults(IEnumerable<SearchBudgetsItem?>? items) => new(
		SearchArgs,
		(items ?? Array.Empty<SearchBudgetsItem>()).
			Where(i => i is not null).
			Select(i => BudgetsRootListItemModel.FromService(i!)).
			ToList()
	);

	public static BudgetsRootModel Empty() => new(
		BudgetsSearchModel.Empty(),
		Array.Empty<BudgetsRootListItemModel>()
	);
}

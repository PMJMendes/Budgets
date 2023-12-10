using Krypton.Budgets.Blazor.APIClient.Budgets.Budget.BudgetDetails;
using Krypton.Budgets.Blazor.APIClient.Global.SearchBudgets;
using Krypton.Budgets.Blazor.UI.Budgets._Common;
using Krypton.Budgets.Blazor.UI.Budgets.BudgetsRoot;

namespace Krypton.Budgets.Blazor.UI.Budgets.BudgetPage.Navigator;

public class BudgetNavigatorModel
{
	private BudgetNavigatorModel(
		BudgetsSearchModel searchArgs,
		IEnumerable<BudgetNavigatorListItemModel> budgets,
		Guid selectedId)
	{
		SearchArgs = searchArgs;
		Budgets = budgets;
		SelectedId = selectedId;
	}

	public BudgetsSearchModel SearchArgs { get; private init; }
	public IEnumerable<BudgetNavigatorListItemModel> Budgets { get; private init; }
	public Guid SelectedId { get; private init; }

	public BudgetNavigatorModel WithId(Guid id) => new(
		SearchArgs,
		Budgets,
		id
	);

	public BudgetNavigatorModel WithSearchArgs(BudgetsSearchModel searchArgs) => new(
		searchArgs,
		Budgets,
		SelectedId
	);

	public BudgetNavigatorModel WithSearchResults(IEnumerable<SearchBudgetsItem?>? items) => new(
		SearchArgs,
		(items ?? Array.Empty<SearchBudgetsItem>()).
			Where(i => i is not null).
			Select(i => BudgetNavigatorListItemModel.FromService(i!)).
			ToList(),
		SelectedId
	);

	public BudgetNavigatorModel WithCreateResults(BudgetDetailsItem? results) => new(
		SearchArgs,
		Budgets.Prepend(BudgetNavigatorListItemModel.FromCreateResults(results)).ToList(),
		results?.Id ?? Guid.Empty
	);

	public BudgetNavigatorModel WithEditResults(BudgetDetailsItem? results) => new(
		SearchArgs,
		Budgets.Select(c => c.Id == (results?.Id ?? Guid.Empty) ? BudgetNavigatorListItemModel.FromEditResults(results) : c).ToList(),
		results?.Id ?? Guid.Empty
	);

	public BudgetNavigatorModel WithSelectedUserState(BudgetState state) => new(
		SearchArgs,
		Budgets.Select(c => c.Id == SelectedId ? c.WithState(state) : c).ToList(),
		SelectedId
	);

	public static BudgetNavigatorModel Empty() => new(
		BudgetsSearchModel.Empty(),
		Array.Empty<BudgetNavigatorListItemModel>(),
		Guid.Empty
	);

	public static BudgetNavigatorModel FromRoot(BudgetsRootModel rootModel) => new(
		rootModel.SearchArgs,
		rootModel.Items.
			Select(i => BudgetNavigatorListItemModel.FromRoot(i)).
			ToList(),
		Guid.Empty
	);
}

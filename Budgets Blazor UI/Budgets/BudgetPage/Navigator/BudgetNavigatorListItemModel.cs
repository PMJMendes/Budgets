using Krypton.Budgets.Blazor.APIClient.Budgets.Budget.BudgetDetails;
using Krypton.Budgets.Blazor.APIClient.Global.SearchBudgets;
using Krypton.Budgets.Blazor.UI.Budgets._Common;
using Krypton.Budgets.Blazor.UI.Budgets.BudgetsRoot;

namespace Krypton.Budgets.Blazor.UI.Budgets.BudgetPage.Navigator;

public class BudgetNavigatorListItemModel
{
	public BudgetNavigatorListItemModel(Guid id, string code, string title, BudgetState state)
	{
		Id = id;
		Code = code;
		Title = title;
		State = state;
	}

	public Guid Id { get; private init; }
	public string Code { get; private init; }
	public string Title { get; private init; }
	public BudgetState State { get; private init; }

	public BudgetNavigatorListItemModel WithState(BudgetState state) => new(
		Id,
		Code,
		Title,
		state
	);

	public static BudgetNavigatorListItemModel Empty() => new(
		Guid.Empty,
		"",
		"",
		BudgetState._UNKNOWN
	);

	public static BudgetNavigatorListItemModel FromRoot(BudgetsRootListItemModel rootModel) => new(
		rootModel.Id,
		rootModel.Code,
		rootModel.Title,
		rootModel.State
	);

	public static BudgetNavigatorListItemModel FromService(SearchBudgetsItem item) => new(
		item.Id ?? Guid.Empty,
		item.Code ?? "",
		item.Title ?? "",
		Enum.TryParse(item.State, true, out BudgetState state) ? state : BudgetState._UNKNOWN
	);

	public static BudgetNavigatorListItemModel FromCreateResults(BudgetDetailsItem? results) => new(
		results?.Id ?? Guid.Empty,
		results?.Code ?? "",
		results?.Title ?? "",
		Enum.TryParse(results?.State, true, out BudgetState state) ? state : BudgetState._UNKNOWN
	);

	public static BudgetNavigatorListItemModel FromEditResults(BudgetDetailsItem? results) => new(
		results?.Id ?? Guid.Empty,
		results?.Code ?? "",
		results?.Title ?? "",
		Enum.TryParse(results?.State, true, out BudgetState state) ? state : BudgetState._UNKNOWN
	);
}

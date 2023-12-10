using Krypton.Budgets.Blazor.APIClient.Budgets.Budget.BudgetDetails;
using Krypton.Budgets.Blazor.APIClient.Budgets.Budget.ProductionDetails;
using Krypton.Budgets.Blazor.UI.Budgets._Common;
using Krypton.Budgets.Blazor.UI.Budgets.BudgetPage.Create;
using Krypton.Budgets.Blazor.UI.Budgets.BudgetPage.Edit;
using Krypton.Budgets.Blazor.UI.Budgets.BudgetPage.Manage;
using Krypton.Budgets.Blazor.UI.Budgets.BudgetPage.Navigator;
using Krypton.Budgets.Blazor.UI.Budgets.BudgetPage.View;
using Krypton.Budgets.Blazor.UI.Budgets.BudgetsRoot;

namespace Krypton.Budgets.Blazor.UI.Budgets.BudgetPage;

public class BudgetPageModel
{
	private BudgetPageModel(bool empty, BudgetNavigatorModel navData, BudgetCreateModel? createData = null,
		BudgetViewModel? viewData = null, BudgetEditModel? editData = null, BudgetManageModel? manageData = null)
	{
		IsNew = empty;
		NavData = navData;

		CreateData = createData;
		ViewData = viewData;
		EditData = editData;
		ManageData = manageData;
	}

	public bool IsNew { get; private init; }
	public BudgetNavigatorModel NavData { get; private init; }
	public BudgetCreateModel? CreateData { get; private init; }
	public BudgetViewModel? ViewData { get; private init; }
	public BudgetEditModel? EditData { get; private init; }
	public BudgetManageModel? ManageData { get; private init; }

	public BudgetPageModel ForCreate() => new(
		IsNew,
		NavData.WithId(Guid.Empty),
		BudgetCreateModel.Empty()
	);

	public BudgetPageModel ForClone() => new(
		IsNew,
		NavData.WithId(Guid.Empty),
		BudgetCreateModel.ForClone(ViewData!)
	);

	public BudgetPageModel WithCreateResults(BudgetDetailsItem? results) => new(
		IsNew,
		NavData.WithCreateResults(results),
		null,
		BudgetViewModel.FromService(results)
	);

	public BudgetPageModel ForView(Guid id) => new(
		IsNew,
		NavData.WithId(id)
	);

	public BudgetPageModel ForEdit() => new(
		IsNew,
		NavData,
		null,
		ViewData,
		BudgetEditModel.FromViewData(ViewData ?? BudgetViewModel.Empty())
	);

	public BudgetPageModel WithEditResults(BudgetDetailsItem? results) => new(
		IsNew,
		NavData.WithEditResults(results),
		null,
		BudgetViewModel.FromService(results)
	);

	public BudgetPageModel WithManageResults(ProductionDetailsItem? results) => new(
		IsNew,
		NavData,
		null,
		ViewData?.WithManageResults(results) ?? BudgetViewModel.FromService(results)
	);

	public BudgetPageModel CancelEdit() => new(
		IsNew,
		NavData,
		null,
		ViewData
	);

	public BudgetPageModel ForManage() => new(
		IsNew,
		NavData,
		null,
		ViewData,
		null,
		BudgetManageModel.FromViewData(ViewData ?? BudgetViewModel.Empty())
	);

	public BudgetPageModel WithSelectedUserState(BudgetState state) => new(
		IsNew,
		NavData.WithSelectedUserState(state),
		CreateData,
		ViewData?.WithState(state),
		EditData
	);

	public static BudgetPageModel Empty() => new(
		true,
		BudgetNavigatorModel.Empty()
	);

	public static BudgetPageModel FromRoot(BudgetsRootModel rootModel, bool forCreate) => new(
		false,
		BudgetNavigatorModel.FromRoot(rootModel),
		forCreate ? BudgetCreateModel.Empty() : null
	);

	public static BudgetPageModel FromResolver(BudgetNavigatorModel navData, BudgetCreateModel? createData,
		BudgetViewModel? viewData = null, BudgetEditModel? editData = null, BudgetManageModel? manageData = null) => new(
		false,
		navData,
		createData,
		viewData,
		editData,
		manageData
	);
}

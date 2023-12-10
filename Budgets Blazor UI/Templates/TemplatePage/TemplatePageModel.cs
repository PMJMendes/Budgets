using Krypton.Budgets.Blazor.APIClient.Budgets.Budget.BudgetDetails;
using Krypton.Budgets.Blazor.UI.Templates.TemplatePage.Create;
using Krypton.Budgets.Blazor.UI.Templates.TemplatePage.Edit;
using Krypton.Budgets.Blazor.UI.Templates.TemplatePage.Navigator;
using Krypton.Budgets.Blazor.UI.Templates.TemplatePage.View;
using Krypton.Budgets.Blazor.UI.Templates.TemplatesRoot;

namespace Krypton.Budgets.Blazor.UI.Templates.TemplatePage;

public class TemplatePageModel
{
	private TemplatePageModel(bool empty, TemplateNavigatorModel navData,
		TemplateCreateModel? createData = null, TemplateViewModel? viewData = null, TemplateEditModel? editData = null)
	{
		IsNew = empty;
		NavData = navData;

		CreateData = createData;
		ViewData = viewData;
		EditData = editData;
	}

	public bool IsNew { get; private init; }
	public TemplateNavigatorModel NavData { get; private init; }
	public TemplateCreateModel? CreateData { get; private init; }
	public TemplateViewModel? ViewData { get; private init; }
	public TemplateEditModel? EditData { get; private init; }

	public TemplatePageModel ForCreate() => new(
		IsNew,
		NavData.WithId(Guid.Empty),
		TemplateCreateModel.Empty()
	);

	public TemplatePageModel ForClone() => new(
		IsNew,
		NavData.WithId(Guid.Empty),
		TemplateCreateModel.ForClone(ViewData!)
	);

	public TemplatePageModel WithCreateResults(BudgetDetailsItem? results) => new(
		IsNew,
		NavData.WithCreateResults(results),
		null,
		TemplateViewModel.FromService(results)
	);

	public TemplatePageModel ForView(Guid id) => new(
		IsNew,
		NavData.WithId(id)
	);

	public TemplatePageModel ForEdit() => new(
		IsNew,
		NavData,
		null,
		ViewData,
		TemplateEditModel.FromViewData(ViewData ?? TemplateViewModel.Empty())
	);

	public TemplatePageModel WithEditResults(BudgetDetailsItem? results) => new(
		IsNew,
		NavData.WithEditResults(results),
		null,
		TemplateViewModel.FromService(results)
	);

	public TemplatePageModel CancelEdit() => new(
		IsNew,
		NavData,
		null,
		ViewData
	);

	public static TemplatePageModel Empty() => new(
		true,
		TemplateNavigatorModel.Empty()
	);

	public static TemplatePageModel FromRoot(TemplatesRootModel rootModel, bool forCreate) => new(
		false,
		TemplateNavigatorModel.FromRoot(rootModel),
		forCreate ? TemplateCreateModel.Empty() : null
	);

	public static TemplatePageModel FromResolver(TemplateNavigatorModel navData, TemplateCreateModel? createData,
		TemplateViewModel? viewData = null, TemplateEditModel? editData = null) => new(
		false,
		navData,
		createData,
		viewData,
		editData
	);
}

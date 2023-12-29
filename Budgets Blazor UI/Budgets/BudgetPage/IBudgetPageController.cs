using Krypton.Budgets.Blazor.APIClient.Budgets.Budget.BudgetDetails;
using Krypton.Budgets.Blazor.APIClient.Budgets.Budget.ProductionDetails;
using Krypton.Budgets.Blazor.UI.Budgets._Common;

namespace Krypton.Budgets.Blazor.UI.Budgets.BudgetPage;

public interface IBudgetPageController
{
	void OnCreate();
	void OnClone();
	Task AfterCreateAsync(BudgetDetailsItem? results);
	void OnCancelCreate();

	void OnView(Guid id);
	Task OnPrint(Guid id, bool isEnglish);
	Task OnReport(Guid id);

	void OnEdit();
	Task AfterEditAsync(BudgetDetailsItem? results);
	void OnCancelEdit();

	void OnManage();
	Task AfterManageAsync(ProductionDetailsItem? results);

	Task OnStateChangedAsync(BudgetState state);

	Task OnDeleteAsync();
}

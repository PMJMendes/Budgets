using Krypton.Budgets.Blazor.APIClient.Budgets.Budget.BudgetDetails;

namespace Krypton.Budgets.Blazor.UI.Templates.TemplatePage;

public interface ITemplatePageController
{
	void OnCreate();
	void OnClone();
	Task AfterCreateAsync(BudgetDetailsItem? results);
	void OnCancelCreate();

	void OnView(Guid id);

	void OnEdit();
	Task AfterEditAsync(BudgetDetailsItem? results);
	void OnCancelEdit();

	Task OnDeleteAsync();
}

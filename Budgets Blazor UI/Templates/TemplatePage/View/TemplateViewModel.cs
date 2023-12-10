using Krypton.Budgets.Blazor.APIClient.Budgets.Budget.BudgetDetails;
using Krypton.Budgets.Blazor.UI.Budgets.BudgetPage._Core;

namespace Krypton.Budgets.Blazor.UI.Templates.TemplatePage.View;

public class TemplateViewModel
{
	private TemplateViewModel(Guid id, string code, BudgetModel budgetData)
	{
		Id = id;
		Code = code;
		TemplateData = budgetData;
	}

	public Guid Id { get; private init; }
	public string Code { get; private init; }
	public BudgetModel TemplateData { get; private init; }

	public static TemplateViewModel Empty() => new(
		Guid.Empty,
		string.Empty,
		BudgetModel.Empty()
	);

	public static TemplateViewModel FromService(BudgetDetailsItem? item) => new(
		item?.Id ?? Guid.Empty,
		item?.Code ?? string.Empty,
		BudgetModel.FromService(item)
	);
}

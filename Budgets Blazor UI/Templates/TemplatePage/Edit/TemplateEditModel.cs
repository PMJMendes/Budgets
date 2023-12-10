﻿using Krypton.Budgets.Blazor.APIClient.Budgets.Budget.UpdateBudget;
using Krypton.Budgets.Blazor.UI.Budgets.BudgetPage._Core;
using Krypton.Budgets.Blazor.UI.Templates.TemplatePage.View;

namespace Krypton.Budgets.Blazor.UI.Templates.TemplatePage.Edit;

public class TemplateEditModel
{
	private TemplateEditModel(Guid id, string code, BudgetModel budgetData)
	{
		Id = id;
		Code = code;
		BudgetData = budgetData;
	}

	public Guid Id { get; private init; }
	public string Code { get; private init; }
	public BudgetModel BudgetData { get; private init; }

	public UpdateBudgetArgs AsArgs() => new(
		Id,
		Code,
		BudgetData.FrontData.AsArgs(),
		BudgetData.PercentsData.AsArgs(),
		BudgetData.Groups.Select(g => g.AsUpdateArgs()).ToList()
	);

	public static TemplateEditModel Empty() => new(
		Guid.Empty,
		string.Empty,
		BudgetModel.Empty()
	);

	public static TemplateEditModel FromViewData(TemplateViewModel viewData) => new(
		viewData.Id,
		viewData.Code,
		viewData.TemplateData.Clone()
	);
}

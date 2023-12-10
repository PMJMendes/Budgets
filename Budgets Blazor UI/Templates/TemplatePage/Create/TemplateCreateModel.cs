using Krypton.Budgets.Blazor.APIClient.Budgets.Budget.CloneBudget;
using Krypton.Budgets.Blazor.APIClient.Global.CreateBudget;
using Krypton.Budgets.Blazor.UI.Templates.TemplatePage.View;

namespace Krypton.Budgets.Blazor.UI.Templates.TemplatePage.Create;

public class TemplateCreateModel
{
	private TemplateCreateModel(Guid sourceId, string code, string title)
	{
		SourceId = sourceId;
		Code = code;
		Title = title;
	}

	public Guid SourceId { get; private init; }
	public string Code { get; set; }
	public string Title { get; set; }

	public CreateBudgetArgs AsCreateArgs() => new(
		Code,
		true,
		DateOnly.FromDateTime(DateTime.Today),
		Title
	);

	public CloneBudgetArgs AsCloneArgs() => new(
		SourceId,
		Code,
		true,
		DateOnly.FromDateTime(DateTime.Today),
		Title
	);

	public TemplateCreateModel WithCode(string code) => new(
		SourceId,
		code,
		Title
	);

	public static TemplateCreateModel Empty() => new(
		Guid.Empty,
		string.Empty,
		string.Empty
	);

	public static TemplateCreateModel ForClone(TemplateViewModel source) => new(
		source.Id,
		string.Empty,
		source.TemplateData.FrontData.Title + " dup."
	);
}

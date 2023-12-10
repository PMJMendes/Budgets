using Krypton.Budgets.Blazor.APIClient.Global.SearchBudgets;
using Krypton.Budgets.Blazor.UI.Budgets._Common;
using System.Globalization;

namespace Krypton.Budgets.Blazor.UI.Budgets.BudgetsRoot;

public class BudgetsRootListItemModel
{
	private BudgetsRootListItemModel(
		Guid id,
		string code,
		DateTime budgetDate,
		string title,
		BudgetState state,
		string? finalClient,
		string? product)
	{
		Id = id;
		Code = code;
		BudgetDate = budgetDate;
		Title = title;
		State = state;
		FinalClient = finalClient;
		Product = product;
	}

	public Guid Id { get; private init; }
	public string Code { get; private init; }
	public DateTime BudgetDate { get; private init; }
	public string Title { get; private init; }
	public BudgetState State { get; private init; }
	public string? FinalClient { get; private init; }
	public string? Product { get; private init; }

	public static BudgetsRootListItemModel Empty() => new(
		Guid.Empty,
		"",
		DateTime.MinValue,
		"",
		BudgetState._UNKNOWN,
		null,
		null
	);

	public static BudgetsRootListItemModel FromService(SearchBudgetsItem item)
	{
		return new(
			item.Id ?? Guid.Empty,
			item.Code ?? "",
			DateTime.TryParse(item.BudgetDate, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind,
				out var date) ? date : DateTime.MinValue,
			item.Title ?? "",
			Enum.TryParse(item.State, true, out BudgetState state) ? state : BudgetState._UNKNOWN,
			item.FinalClient,
			item.Product
		);
	}
}

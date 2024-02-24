using Krypton.Budgets.Blazor.APIClient._Impl;
using Krypton.Budgets.Blazor.APIClient.Budgets.Budget.BudgetDetails;
using Krypton.Budgets.Blazor.APIClient.Budgets.Budget.ProductionDetails;
using Krypton.Budgets.Blazor.UI._Shared;
using Krypton.Budgets.Blazor.UI.Budgets._Common;
using Krypton.Budgets.Blazor.UI.Budgets.BudgetPage._Core;
using System.Text;

namespace Krypton.Budgets.Blazor.UI.Budgets.BudgetPage.View;

public class BudgetViewModel
{
	private BudgetViewModel(Guid id, string code, string? owner, RefModel? manager,
		BudgetModel budgetData, BudgetState state)
	{
		Id = id;
		Code = code;
		Owner = owner;
		Manager = manager;
		BudgetData = budgetData;
		State = state;
	}

	public Guid Id { get; private init; }
	public string Code { get; private init; }
	public string? Owner { get; private init; }
	public RefModel? Manager { get; private init; }
	public BudgetModel BudgetData { get; private init; }
	public BudgetState State { get; private init; }

	public Stream GetCSVStream()
	{
		MemoryStream responseDataStream = new MemoryStream();

		using (StreamWriter sw = new StreamWriter(responseDataStream, Encoding.UTF8, -1, true))
		{
			BudgetData.WriteToCSVStream(sw);
			sw.Flush();
			sw.Close();
		}

		responseDataStream.Position = 0;
		return responseDataStream;
	}

	public BudgetViewModel WithState(BudgetState state) => new(
		Id,
		Code,
		Owner,
		Manager,
		BudgetData,
		state
	);

	public BudgetViewModel WithManager(RefModel? manager) => new(
		Id,
		Code,
		Owner,
		manager,
		BudgetData,
		State
	);

	public BudgetViewModel WithManageResults(ProductionDetailsItem? results) => new(
		Id,
		Code,
		Owner,
		Manager,
		BudgetData.WithManageResults(results),
		State
	);

	public static BudgetViewModel Empty() => new(
		Guid.Empty,
		string.Empty,
		null,
		null,
		BudgetModel.Empty(),
		BudgetState._UNKNOWN
	);

	public static BudgetViewModel FromService(BudgetDetailsItem? item) => new(
		item?.Id ?? Guid.Empty,
		item?.Code ?? string.Empty,
		item?.Owner,
		item?.Manager is RefItem manager ? RefModel.FromRefItem(manager) : null,
		BudgetModel.FromService(item),
		Enum.TryParse(item?.State, true, out BudgetState state) ? state : BudgetState._UNKNOWN
	);

	public static BudgetViewModel FromService(ProductionDetailsItem? item) => new(
		item?.Id ?? Guid.Empty,
		item?.Code ?? string.Empty,
		null,
		null,
		BudgetModel.FromService(item),
		BudgetState._UNKNOWN
	);
}

using Krypton.Budgets.Blazor.APIClient.Budgets.Budget.BudgetDetails;
using Krypton.Budgets.Blazor.APIClient.Budgets.Budget.ProductionDetails;
using Krypton.Budgets.Shared;

namespace Krypton.Budgets.Blazor.UI.Budgets.BudgetPage._Core;

public class BudgetModel : IBudget
{
	private BudgetModel(BudgetFrontModel frontData, BudgetPercentsModel percentsData, IEnumerable<GroupModel> groups)
	{
		FrontData = frontData.WithOwner(this);
		PercentsData = percentsData;
		Groups = groups.ToList();

		((IBudget)this).RecalculateClientTotal();
		((IBudget)this).RecalculateProd();
	}

	public BudgetFrontModel FrontData { get; private set; }
	public BudgetPercentsModel PercentsData { get; private set; }
	public List<GroupModel> Groups { get; private init; }

	string? IBudget.StudioDays => FrontData.StudioDays;
	string? IBudget.LocationDays => FrontData.LocationDays;
	string? IBudget.OutsideDays => FrontData.OutsideDays;
	string? IBudget.WeekendHolidays => FrontData.WeekendHolidays;
	decimal? IBudget.ProducerPercent => PercentsData.ProducerPercent;
	decimal? IBudget.BCAPercent => PercentsData.BCAPercent;
	decimal? IBudget.WeatherTotal => PercentsData.WeatherTotal;
	int? IBudget.NWeatherDays => PercentsData.NWeatherDays;

	IEnumerable<IGroup> IBudget.Groups => Groups;

	public void FixWeatherTotal() => PercentsData.FixWeatherTotal(((IBudget)this).Value);

	public GroupModel AddGroup(GroupModel group)
	{
		Groups.Add(group);
		return group;
	}

	public void RemoveGroup(GroupModel group) => Groups.Remove(group);

	public BudgetModel WithManageResults(ProductionDetailsItem? results) => new(
		FrontData,
		PercentsData.WithManageResults(results?.NWeatherDays),
		UpdateGroupProdData(results?.Groups?.ToList() ?? new())
	);

	public BudgetModel Clone() => new(
		FrontData.Clone(),
		PercentsData.Clone(),
		Groups.Select(g => g.Clone()).ToList()
	);

	public static BudgetModel Empty() => new(
		BudgetFrontModel.Empty(),
		BudgetPercentsModel.Empty(),
		new List<GroupModel>()
	);

	public static BudgetModel FromService(BudgetDetailsItem? item) => new(
		BudgetFrontModel.FromService(item),
		BudgetPercentsModel.FromService(item),
		item?.Groups?.Select(g => GroupModel.FromService(g)) ?? Array.Empty<GroupModel>()
	);

	public static BudgetModel FromService(ProductionDetailsItem? item) => new(
		BudgetFrontModel.FromService(item),
		BudgetPercentsModel.FromService(item),
		item?.Groups?.Select(g => GroupModel.FromService(g)) ?? Array.Empty<GroupModel>()
	);

	private IEnumerable<GroupModel> UpdateGroupProdData(List<PGroupItem?> groups) =>
		Groups.Select((g, i) => i < groups.Count && groups[i] is PGroupItem item && g.Id == item.Id ?
			g.WithManageResults(item) :
			g
		);
}

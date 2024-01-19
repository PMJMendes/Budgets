using Krypton.Budgets.Blazor.APIClient.Budgets.Budget.BudgetDetails;
using Krypton.Budgets.Blazor.APIClient.Budgets.Budget.ProductionDetails;
using Krypton.Budgets.Blazor.APIClient.Budgets.Budget.UpdateBudget;

namespace Krypton.Budgets.Blazor.UI.Budgets.BudgetPage._Core;

public class BudgetPercentsModel
{
	private BudgetPercentsModel(decimal? producerPercent, decimal? bCAPercent,
		int? nWeatherDays, decimal? weatherTotal, decimal? weatherPercent)
	{
		ProducerPercent = producerPercent;
		BCAPercent = bCAPercent;
		NWeatherDays = nWeatherDays;
		WeatherTotal = weatherTotal;
		WeatherPercent = weatherPercent;
	}

	public decimal? ProducerPercent { get; set; }
	public decimal? BCAPercent { get; set; }
	public int? NWeatherDays { get; set; }
	public decimal? WeatherTotal { get; set; }
	public decimal? WeatherPercent { get; set; }

	public void FixWeatherTotal(decimal total) => WeatherTotal = WeatherPercent is decimal percent ? total * percent / 100m : WeatherTotal;

	public FinalDataArgs AsArgs() => new(
		ProducerPercent,
		BCAPercent,
		NWeatherDays,
		WeatherTotal,
		WeatherPercent
	);
	public BudgetPercentsModel WithManageResults(int? nWeatherDays) => new(
		ProducerPercent, BCAPercent,
		nWeatherDays,
		WeatherTotal,
		WeatherPercent
	);

	public BudgetPercentsModel Clone() => new(
		ProducerPercent, BCAPercent,
		NWeatherDays, WeatherTotal, WeatherPercent
	);

	public static BudgetPercentsModel Empty() => new(
		null, null,
		null, null, null
	);

	public static BudgetPercentsModel FromService(BudgetDetailsItem? item) => new(
		item?.ProducerPercent, item?.BCAPercent,
		item?.NWeatherDays, item?.WeatherTotal, item?.WeatherPercent
	);

	public static BudgetPercentsModel FromService(ProductionDetailsItem? item) => new(
		null, null,
		item?.NWeatherDays, item?.WeatherTotal, item?.WeatherPercent
	);
}

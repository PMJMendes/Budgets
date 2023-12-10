namespace Krypton.Budgets.Domain.Budgets.Budget_.UpdateBudget;

public interface IFinalDataArgs
{
    decimal? ProducerPercent { get; }
    decimal? BCAPercent { get; }

    int? NWeatherDays { get; }
    decimal? WeatherTotal { get; }
    decimal? WeatherPercent { get; }

    internal sealed BudgetFinalData ToFinalData() => new(
        ProducerPercent, BCAPercent,
        NWeatherDays, WeatherTotal, WeatherPercent
    );
}

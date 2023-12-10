namespace Krypton.Budgets.Domain.Budgets.Budget_;

internal record struct BudgetFinalData(
    decimal? ProducerPercent, decimal? BCAPercent,
    int? NWeatherDays, decimal? WeatherTotal, decimal? WeatherPercent
);

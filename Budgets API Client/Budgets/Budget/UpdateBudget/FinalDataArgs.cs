namespace Krypton.Budgets.Blazor.APIClient.Budgets.Budget.UpdateBudget;

public class FinalDataArgs
{
    public FinalDataArgs(decimal? producerPercent, decimal? bCAPercent,
        int? nWeatherDays, decimal? weatherTotal, decimal? weatherPercent)
    {
        ProducerPercent = producerPercent;
        BCAPercent = bCAPercent;

        NWeatherDays = nWeatherDays;
        WeatherTotal = weatherTotal;
        WeatherPercent = weatherPercent;
    }

    public decimal? ProducerPercent { get; private init; }
    public decimal? BCAPercent { get; private init; }
    public int? NWeatherDays { get; private init; }
    public decimal? WeatherTotal { get; private init; }
    public decimal? WeatherPercent { get; private init; }
}

using Krypton.Budgets.Domain.Budgets.Budget_.UpdateBudget;

namespace Krypton.Budgets.API.Budgets.Budget.UpdateBudget;

internal class FinalDataArgs : IFinalDataArgs
{
    public decimal? ProducerPercent { get; set; }
    public decimal? BCAPercent { get; set; }
    public int? NWeatherDays { get; set; }
    public decimal? WeatherTotal { get; set; }
    public decimal? WeatherPercent { get; set; }
}

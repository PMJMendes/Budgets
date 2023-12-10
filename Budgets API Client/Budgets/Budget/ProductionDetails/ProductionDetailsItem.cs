namespace Krypton.Budgets.Blazor.APIClient.Budgets.Budget.ProductionDetails;

public class ProductionDetailsItem
{
    public Guid? Id { get; set; }
    public string? Code { get; set; }
    public string? BudgetDate { get; set; }
    public string? Title { get; set; }
    public string? FilmDate { get; set; }
    public string? FinalClient { get; set; }
    public string? Product { get; set; }
    public string? Agency { get; set; }
    public string? Director { get; set; }
    public string? Producer { get; set; }
    public string? TVAgency { get; set; }
    public string? Rights { get; set; }
    public string? StudioDays { get; set; }
    public string? LocationDays { get; set; }
    public string? OutsideDays { get; set; }
    public string? WeekendHolidays { get; set; }
    public string? PostProdDuration { get; set; }
    public string? PostProdVersions { get; set; }
    public string? PostProdSound { get; set; }
    public string? PostProdNVoices { get; set; }
    public string? Comments { get; set; }
    public string? CommentsEnglish { get; set; }
    public int? NWeatherDays { get; set; }
    public decimal? WeatherTotal { get; set; }
    public decimal? WeatherPercent { get; set; }
    public IEnumerable<PGroupItem?>? Groups { get; set; }
}

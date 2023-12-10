using Krypton.Budgets.Blazor.APIClient._Impl;

namespace Krypton.Budgets.Blazor.APIClient.Budgets.Budget.BudgetDetails;

public class BudgetDetailsItem
{
    public Guid? Id { get; set; }
    public string? Code { get; set; }
    public bool? IsTemplate { get; set; }
    public string? BudgetDate { get; set; }
    public string? Title { get; set; }
    public string? State { get; set; }
    public string? FilmDate { get; set; }
    public string? FinalClient { get; set; }
    public string? Product { get; set; }
    public string? Agency { get; set; }
    public string? Director { get; set; }
    public string? Producer { get; set; }
    public string? TVAgency { get; set; }
    public string? Rights { get; set; }
    public string? Formats { get; set; }
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
    public decimal? ProducerPercent { get; set; }
    public decimal? BCAPercent { get; set; }
    public int? NWeatherDays { get; set; }
    public decimal? WeatherTotal { get; set; }
    public decimal? WeatherPercent { get; set; }
    public IEnumerable<GroupItem?>? Groups { get; set; }
    public string? Owner { get; set; }
    public RefItem? Manager { get; set; }
}

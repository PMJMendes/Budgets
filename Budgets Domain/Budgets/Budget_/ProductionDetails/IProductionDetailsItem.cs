using Krypton.Budgets.Domain._Base.Interfaces;

namespace Krypton.Budgets.Domain.Budgets.Budget_.ProductionDetails;

public interface IProductionDetailsItem : IQueryResultItem
{
    Guid Id { get; }

    string Code { get; }
    DateOnly BudgetDate { get; }
    string Title { get; }

    string? FilmDate { get; }
    string? FinalClient { get; }
    string? Product { get; }
    string? Agency { get; }
    string? Director { get; }
    string? Producer { get; }
    string? TVAgency { get; }
    string? Rights { get; }

    string? StudioDays { get; }
    string? LocationDays { get; }
    string? OutsideDays { get; }
    string? WeekendHolidays { get; }

    string? PostProdDuration { get; }
    string? PostProdVersions { get; }
    string? PostProdSound { get; }
    string? PostProdNVoices { get; }

    string? Comments { get; }
    string? CommentsEnglish { get; }

    int? NWeatherDays { get; }
    decimal? WeatherTotal { get; }
    decimal? WeatherPercent { get; }

    IEnumerable<IPGroupItem> Groups { get; }
}

using Krypton.Budgets.Domain._Base.Interfaces;

namespace Krypton.Budgets.Domain.Budgets.Budget_.BudgetDetails;

public interface IBudgetDetailsItem : IQueryResultItem
{
    Guid Id { get; }

    string Code { get; }
    bool IsTemplate { get; }
    DateOnly BudgetDate { get; }
    string Title { get; }
    BudgetState State { get; }

    string? FilmDate { get; }
    string? FinalClient { get; }
    string? Product { get; }
    string? Agency { get; }
    string? Director { get; }
    string? Producer { get; }
    string? TVAgency { get; }
    string? Rights { get; }
    string? Formats { get; }

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

    decimal? ProducerPercent { get; }
    decimal? BCAPercent { get; }

    int? NWeatherDays { get; }
    decimal? WeatherTotal { get; }
    decimal? WeatherPercent { get; }

    IEnumerable<IGroupItem> Groups { get; }

    string? Owner { get; }
    IRefItem? Manager { get; }
}

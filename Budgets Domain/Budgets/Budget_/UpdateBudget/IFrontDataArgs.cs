using Krypton.Budgets.Domain._Impl.Attributes;

namespace Krypton.Budgets.Domain.Budgets.Budget_.UpdateBudget;

public interface IFrontDataArgs
{
    [Required]
    DateOnly? BudgetDate { get; }

    [Required]
    string? Title { get; }

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

    internal sealed BudgetFrontData ToFrontData(Exception ex) => new(
        BudgetDate ?? throw ex, Title ?? throw ex,
        FilmDate, FinalClient, Product, Agency,
        Director, Producer, TVAgency, Rights,
        StudioDays, LocationDays, OutsideDays, WeekendHolidays,
        PostProdDuration, PostProdVersions, PostProdSound, PostProdNVoices,
        Comments, CommentsEnglish
    );
}

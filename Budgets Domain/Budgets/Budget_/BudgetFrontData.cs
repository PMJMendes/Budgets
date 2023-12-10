namespace Krypton.Budgets.Domain.Budgets.Budget_;

internal record struct BudgetFrontData(
    DateOnly BudgetDate, string Title,
    string? FilmDate, string? FinalClient, string? Product, string? Agency,
    string? Director, string? Producer, string? TVAgency, string? Rights,
    string? StudioDays, string? LocationDays, string? OutsideDays, string? WeekendHolidays,
    string? PostProdDuration, string? PostProdVersions, string? PostProdSound, string? PostProdNVoices,
    string? Comments, string? CommentsEnglish
);

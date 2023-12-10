using Krypton.Budgets.Domain.Budgets.Budget_.UpdateBudget;

namespace Krypton.Budgets.API.Budgets.Budget.UpdateBudget;

internal class FrontDataArgs : IFrontDataArgs
{
    public DateOnly? BudgetDate { get; set; }
    public string? Title { get; set; }
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
}

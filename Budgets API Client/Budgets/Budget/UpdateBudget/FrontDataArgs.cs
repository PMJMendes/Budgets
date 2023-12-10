namespace Krypton.Budgets.Blazor.APIClient.Budgets.Budget.UpdateBudget;

public class FrontDataArgs
{
    public FrontDataArgs(DateOnly budgetDate, string title,
        string? filmDate, string? finalClient, string? product, string? agency,
        string? director, string? producer, string? tVAgency, string? rights, string? formats,
        string? studioDays, string? locationDays, string? outsideDays, string? weekendHolidays,
        string? postProdDuration, string? postProdVersions, string? postProdSound, string? postProdNVoices,
        string? comments, string? commentsEnglish)
    {
        BudgetDate = budgetDate;

        Title = title;

        FilmDate = filmDate;
        FinalClient = finalClient;
        Product = product;
        Agency = agency;

        Director = director;
        Producer = producer;
        TVAgency = tVAgency;
        Rights = rights;
        Formats = formats;

        StudioDays = studioDays;
        LocationDays = locationDays;
        OutsideDays = outsideDays;
        WeekendHolidays = weekendHolidays;

        PostProdDuration = postProdDuration;
        PostProdVersions = postProdVersions;
        PostProdSound = postProdSound;
        PostProdNVoices = postProdNVoices;

        Comments = comments;
        CommentsEnglish = commentsEnglish;
    }

    public DateOnly BudgetDate { get; private init; }
    public string Title { get; private init; }
    public string? FilmDate { get; private init; }
    public string? FinalClient { get; private init; }
    public string? Product { get; private init; }
    public string? Agency { get; private init; }
    public string? Director { get; private init; }
    public string? Producer { get; private init; }
    public string? TVAgency { get; private init; }
    public string? Rights { get; private init; }
    public string? Formats { get; private init; }
    public string? StudioDays { get; private init; }
    public string? LocationDays { get; private init; }
    public string? OutsideDays { get; private init; }
    public string? WeekendHolidays { get; private init; }
    public string? PostProdDuration { get; private init; }
    public string? PostProdVersions { get; private init; }
    public string? PostProdSound { get; private init; }
    public string? PostProdNVoices { get; private init; }
    public string? Comments { get; private init; }
    public string? CommentsEnglish { get; private init; }
}

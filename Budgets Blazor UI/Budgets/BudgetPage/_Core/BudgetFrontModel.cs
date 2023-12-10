using Krypton.Budgets.Blazor.APIClient.Budgets.Budget.BudgetDetails;
using Krypton.Budgets.Blazor.APIClient.Budgets.Budget.ProductionDetails;
using Krypton.Budgets.Blazor.APIClient.Budgets.Budget.UpdateBudget;
using Krypton.Budgets.Shared;

namespace Krypton.Budgets.Blazor.UI.Budgets.BudgetPage._Core;

public class BudgetFrontModel
{
    private readonly IBudget _owner;

    private BudgetFrontModel(IBudget owner, DateOnly budgetDate, string title,
        string? filmDate, string? finalClient, string? product, string? agency,
        string? director, string? producer, string? tVAgency, string? rights,
        string? studioDays, string? locationDays, string? outsideDays, string? weekendHolidays,
        string? postProdDuration, string? postProdVersions, string? postProdSound, string? postProdNVoices,
        string? comments, string? commentsEnglish)
    {
        _owner = owner;

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

    public DateOnly BudgetDate { get; set; }
    public string Title { get; set; }
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

    public decimal Value => _owner.Value;
    public decimal ClientValue => _owner.ClientValue;
    public int TotalDays => _owner.TotalDays;

    public FrontDataArgs AsArgs() => new(
        BudgetDate,
        Title,
        FilmDate,
        FinalClient,
        Product,
        Agency,
        Director,
        Producer,
        TVAgency,
        Rights,
        StudioDays,
        LocationDays,
        OutsideDays,
        WeekendHolidays,
        PostProdDuration,
        PostProdVersions,
        PostProdSound,
        PostProdNVoices,
        Comments,
        CommentsEnglish
    );

    public BudgetFrontModel WithOwner(BudgetModel owner) => new(
        owner, BudgetDate, Title,
        FilmDate, FinalClient, Product, Agency,
        Director, Producer, TVAgency, Rights,
        StudioDays, LocationDays, OutsideDays, WeekendHolidays,
        PostProdDuration, PostProdVersions, PostProdSound, PostProdNVoices,
        Comments, CommentsEnglish
    );

    public BudgetFrontModel Clone() => new(
        _owner, BudgetDate, Title,
        FilmDate, FinalClient, Product, Agency,
        Director, Producer, TVAgency, Rights,
        StudioDays, LocationDays, OutsideDays, WeekendHolidays,
        PostProdDuration, PostProdVersions, PostProdSound, PostProdNVoices,
        Comments, CommentsEnglish
    );

    public static BudgetFrontModel Empty() => new(null!,
        DateOnly.FromDateTime(DateTime.UtcNow), string.Empty,
        null, null, null, null,
        null, null, null, null,
        null, null, null, null,
        null, null, null, null,
        null, null
    );

    public static BudgetFrontModel FromService(BudgetDetailsItem? item) => new(null!,
        DateOnly.FromDateTime(DateTime.TryParse(item?.BudgetDate, out DateTime date) ? date : DateTime.UtcNow),
        item?.Title ?? string.Empty,
        item?.FilmDate, item?.FinalClient, item?.Product, item?.Agency,
        item?.Director, item?.Producer, item?.TVAgency, item?.Rights,
        item?.StudioDays, item?.LocationDays, item?.OutsideDays, item?.WeekendHolidays,
        item?.PostProdDuration, item?.PostProdVersions, item?.PostProdSound, item?.PostProdNVoices,
        item?.Comments, item?.CommentsEnglish
    );

    public static BudgetFrontModel FromService(ProductionDetailsItem? item) => new(null!,
        DateOnly.FromDateTime(DateTime.TryParse(item?.BudgetDate, out DateTime date) ? date : DateTime.UtcNow),
        item?.Title ?? string.Empty,
        item?.FilmDate, item?.FinalClient, item?.Product, item?.Agency,
        item?.Director, item?.Producer, item?.TVAgency, item?.Rights,
        item?.StudioDays, item?.LocationDays, item?.OutsideDays, item?.WeekendHolidays,
        item?.PostProdDuration, item?.PostProdVersions, item?.PostProdSound, item?.PostProdNVoices,
        item?.Comments, item?.CommentsEnglish
    );
}

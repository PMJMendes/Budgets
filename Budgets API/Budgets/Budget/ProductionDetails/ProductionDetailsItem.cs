using Krypton.Budgets.Domain.Budgets.Budget_.ProductionDetails;
using System.Text.Json.Serialization;

namespace Krypton.Budgets.API.Budgets.Budget.ProductionDetails;

internal readonly struct ProductionDetailsItem : IProductionDetailsItem
{
    public ProductionDetailsItem(IProductionDetailsItem source)
    {
        Id = source.Id;
        Code = source.Code;
        BudgetDate = source.BudgetDate;
        Title = source.Title;
        FilmDate = source.FilmDate;
        FinalClient = source.FinalClient;
        Product = source.Product;
        Agency = source.Agency;
        Director = source.Director;
        Producer = source.Producer;
        TVAgency = source.TVAgency;
        Rights = source.Rights;
        StudioDays = source.StudioDays;
        LocationDays = source.LocationDays;
        OutsideDays = source.OutsideDays;
        WeekendHolidays = source.WeekendHolidays;
        PostProdDuration = source.PostProdDuration;
        PostProdVersions = source.PostProdVersions;
        PostProdSound = source.PostProdSound;
        PostProdNVoices = source.PostProdNVoices;
        Comments = source.Comments;
        CommentsEnglish = source.CommentsEnglish;
        NWeatherDays = source.NWeatherDays;
        WeatherTotal = source.WeatherTotal;
        WeatherPercent = source.WeatherPercent;
        Groups = source.Groups.Select(g => new PGroupItem(g));
    }

    public Guid Id { get; private init; }
    public string Code { get; private init; }
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
    public int? NWeatherDays { get; private init; }
    public decimal? WeatherTotal { get; private init; }
    public decimal? WeatherPercent { get; private init; }
    public IEnumerable<PGroupItem> Groups { get; private init; }

    [JsonIgnore]
    IEnumerable<IPGroupItem> IProductionDetailsItem.Groups => throw new NotImplementedException();
}

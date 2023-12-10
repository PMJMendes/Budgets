using Krypton.Budgets.API._Impl;
using Krypton.Budgets.Domain._Base.Interfaces;
using Krypton.Budgets.Domain.Budgets.Budget_;
using Krypton.Budgets.Domain.Budgets.Budget_.BudgetDetails;
using System.Text.Json.Serialization;

namespace Krypton.Budgets.API.Budgets.Budget.BudgetDetails;

internal readonly struct BudgetDetailsItem : IBudgetDetailsItem
{
    public BudgetDetailsItem(IBudgetDetailsItem source)
    {
        Id = source.Id;
        Code = source.Code;
        IsTemplate = source.IsTemplate;
        BudgetDate = source.BudgetDate;
        Title = source.Title;
        State = source.State;
        FilmDate = source.FilmDate;
        FinalClient = source.FinalClient;
        Product = source.Product;
        Agency = source.Agency;
        Director = source.Director;
        Producer = source.Producer;
        TVAgency = source.TVAgency;
        Rights = source.Rights;
        Formats = source.Formats;
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
        ProducerPercent = source.ProducerPercent;
        BCAPercent = source.BCAPercent;
        NWeatherDays = source.NWeatherDays;
        WeatherTotal = source.WeatherTotal;
        WeatherPercent = source.WeatherPercent;
        Groups = source.Groups.Select(g => new GroupItem(g));
        Owner = source.Owner;
        Manager = source.Manager is IRefItem manager ? new RefItem(manager) : null;
    }

    public Guid Id { get; private init; }
    public string Code { get; private init; }
    public bool IsTemplate { get; private init; }
    public DateOnly BudgetDate { get; private init; }
    public string Title { get; private init; }
    public BudgetState State { get; private init; }
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
    public decimal? ProducerPercent { get; private init; }
    public decimal? BCAPercent { get; private init; }
    public int? NWeatherDays { get; private init; }
    public decimal? WeatherTotal { get; private init; }
    public decimal? WeatherPercent { get; private init; }
    public IEnumerable<GroupItem> Groups { get; private init; }
    public string? Owner { get; private init; }
    public RefItem? Manager { get; private init; }

    [JsonIgnore]
    IEnumerable<IGroupItem> IBudgetDetailsItem.Groups => throw new NotImplementedException();
    [JsonIgnore]
    IRefItem? IBudgetDetailsItem.Manager => throw new NotImplementedException();
}

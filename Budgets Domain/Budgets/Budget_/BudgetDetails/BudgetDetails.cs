using Krypton.Budgets.Domain._Base.Interfaces;
using Krypton.Budgets.Domain._Base.Services;
using Krypton.Budgets.Domain._Impl.Interfaces;
using Krypton.Budgets.Domain.Budgets.Category_;
using Krypton.Budgets.Domain.Budgets.Cost_;
using Krypton.Budgets.Domain.Budgets.Group_;
using Krypton.Budgets.Domain.Budgets.Invoice_;
using Krypton.Budgets.Domain.Budgets.Item_;
using Krypton.Budgets.Domain.Budgets.Value_;
using Krypton.Budgets.Domain.Budgets.ValueDef_;
using Krypton.Budgets.Domain.Users.User_;
using Microsoft.Extensions.Logging;
using ValueType = Krypton.Budgets.Shared.ValueType;

namespace Krypton.Budgets.Domain.Budgets.Budget_.BudgetDetails;

internal class BudgetDetails : BaseQuery<BudgetDetails, ITargetArgs, IBudgetDetailsItem>, IBudgetDetails
{
    public BudgetDetails(IContext context, ILogger<BudgetDetails> logger) : base(context, logger) { }

    protected override void AssertPermissions() => AssertIsProducer();

    protected override async IAsyncEnumerable<V> InnerFetch<V>(ITargetArgs? args, Func<IBudgetDetailsItem, V> itemFactory)
    {
        var budget = await GetTarget<Budget>(args);

        var creator = context.Persistence.Query<User>().
            Where(u => u.Email == budget.WhoCreated).
            FirstOrDefault();

        yield return itemFactory(BuildBudgetItem(budget, creator));
    }

    private static IBudgetDetailsItem BuildBudgetItem(Budget budget, User? creator) => new BudgetItem(
        budget.Id, budget.Code, budget.IsTemplate,
        budget.BudgetDate, budget.Title, budget.State,
        budget.FilmDate, budget.FinalClient, budget.Product, budget.Agency,
        budget.Director, budget.Producer, budget.TVAgency, budget.Rights, budget.Formats,
        budget.StudioDays, budget.LocationDays, budget.OutsideDays, budget.WeekendHolidays,
        budget.PostProdDuration, budget.PostProdVersions, budget.PostProdSound, budget.PostProdNVoices,
        budget.Comments, budget.CommentsEnglish, budget.ProducerPercent, budget.BCAPercent,
        budget.NWeatherDays, budget.WeatherTotal, budget.WeatherPercent,
        budget.Groups.Select(g => BuildGroupItem(g)),
        creator?.FullName, BuildManagerItem(budget.Manager)
    );

    private static IGroupItem BuildGroupItem(Group g) => new GroupItem(
        g.Id,
        g.Description, g.DescEnglish,
        g.Categories.Select(c => BuildCategoryItem(c))
    );

    private static ICategoryItem BuildCategoryItem(Category c) => new CategoryItem(
        c.Id,
        c.Formula, c.Description, c.DescEnglish,
        c.Defs.Select(d => BuildValueDefItem(d)),
        c.Items.Select(i => BuildItemItem(i))
    );

    private static IValueDefItem BuildValueDefItem(ValueDef d) => new ValueDefItem(
        d.Id,
        d.Type, d.Description, d.DescEnglish, d.BCAFormula
    );

    private static IItemItem BuildItemItem(Item i) => new ItemItem(
        i.Id,
        i.ExcludeFromBase,
        i.CanBePercent, i.Description, i.DescEnglish,
        i.Percent, i.BCAPercent,
        i.Values.Select(v => BuildValueItem(v)),
        i.Costs.Select(c => BuildCostItem(c)),
        i.Invoices.Select(inv => BuildInvoiceItem(inv))
    );

    private static IValueItem BuildValueItem(Value v) => new ValueItem(
        v.Id,
        v.NumberValue, v.TextValue, v.TextEnglish, v.ProdValue
    );

    private static ICostItem BuildCostItem(Cost c) => new CostItem(
        c.Id,
        c.CostValue, c.Supplier, c.Text
    );

    private static IInvoiceItem BuildInvoiceItem(Invoice i) => new InvoiceItem(
        i.Id,
        i.InvoicedValue, i.InvoiceNumber, i.Supplier, i.Text
    );

    private static IRefItem? BuildManagerItem(User? user) => user is User _user ? new RefItem(
        _user.Id,
        _user.FullName
    ) : null;

    private record struct BudgetItem(
        Guid Id, string Code, bool IsTemplate,
        DateOnly BudgetDate, string Title, BudgetState State,
        string? FilmDate, string? FinalClient, string? Product, string? Agency,
        string? Director, string? Producer, string? TVAgency, string? Rights, string? Formats,
        string? StudioDays, string? LocationDays, string? OutsideDays, string? WeekendHolidays,
        string? PostProdDuration, string? PostProdVersions, string? PostProdSound, string? PostProdNVoices,
        string? Comments, string? CommentsEnglish, decimal? ProducerPercent, decimal? BCAPercent,
        int? NWeatherDays, decimal? WeatherTotal, decimal? WeatherPercent,
        IEnumerable<IGroupItem> Groups,
        string? Owner, IRefItem? Manager
    ) : IBudgetDetailsItem;

    private record struct GroupItem(
        Guid Id,
        string Description, string? DescEnglish,
        IEnumerable<ICategoryItem> Categories
    ) : IGroupItem;

    private record struct CategoryItem(
        Guid Id,
        string Formula, string Description, string? DescEnglish,
        IEnumerable<IValueDefItem> Defs,
        IEnumerable<IItemItem> Items
    ) : ICategoryItem;

    private record struct ValueDefItem(
        Guid Id,
        ValueType Type, string Description, string? DescEnglish, string? BCAFormula
    ) : IValueDefItem;

    private record struct ItemItem(
        Guid Id,
        bool ExcludeFromBase, bool CanBePercent, string Description, string? DescEnglish,
        decimal? Percent, decimal? BCAPercent,
        IEnumerable<IValueItem> Values,
        IEnumerable<ICostItem> Costs,
        IEnumerable<IInvoiceItem> Invoices
    ) : IItemItem;

    private record struct ValueItem(
        Guid Id,
        decimal? NumberValue, string? TextValue, string? TextEnglish, decimal? ProdValue
    ) : IValueItem;

    private record struct CostItem(
        Guid Id,
        decimal CostValue, string? Supplier, string? Text
    ) : ICostItem;

    private record struct InvoiceItem(
        Guid Id,
        decimal InvoicedValue, string? InvoiceNumber, string? Supplier, string? Text
    ) : IInvoiceItem;

    private record struct RefItem(
        Guid Id,
        string Description
    ) : IRefItem;
}

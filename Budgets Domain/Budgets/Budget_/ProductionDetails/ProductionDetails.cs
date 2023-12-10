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

namespace Krypton.Budgets.Domain.Budgets.Budget_.ProductionDetails;

internal class ProductionDetails : BaseQuery<ProductionDetails, ITargetArgs, IProductionDetailsItem>, IProductionDetails
{
    public ProductionDetails(IContext context, ILogger<ProductionDetails> logger) : base(context, logger) { }

    protected override void AssertPermissions() => AssertIsLoggedIn();

    protected override async IAsyncEnumerable<V> InnerFetch<V>(ITargetArgs? args, Func<IProductionDetailsItem, V> itemFactory)
    {
        var budget = await GetTarget<Budget>(args);

        if (budget.State != BudgetState.LOCKED || budget.Manager != context.CurrentUser)
        {
            AssertIsProducer();
        }

        var creator = context.Persistence.Query<User>().
            Where(u => u.Email == budget.WhoCreated).
            FirstOrDefault();

        yield return itemFactory(BuildBudgetItem(budget, creator));
    }

    private static IProductionDetailsItem BuildBudgetItem(Budget budget, User? creator) => new BudgetItem(
        budget.Id, budget.Code,
        budget.BudgetDate, budget.Title,
        budget.FilmDate, budget.FinalClient, budget.Product, budget.Agency,
        budget.Director, budget.Producer, budget.TVAgency, budget.Rights,
        budget.StudioDays, budget.LocationDays, budget.OutsideDays, budget.WeekendHolidays,
        budget.PostProdDuration, budget.PostProdVersions, budget.PostProdSound, budget.PostProdNVoices,
        budget.Comments, budget.CommentsEnglish,
        budget.NWeatherDays, budget.WeatherTotal, budget.WeatherPercent,
        budget.Groups.Select(g => BuildGroupItem(g)),
        creator?.FullName, BuildManagerItem(budget.Manager)
    );

    private static IPGroupItem BuildGroupItem(Group g) => new GroupItem(
        g.Id,
        g.Description, g.DescEnglish,
        g.Categories.Select(c => BuildCategoryItem(c))
    );

    private static IPCategoryItem BuildCategoryItem(Category c) => new CategoryItem(
        c.Id,
        c.Formula, c.Description, c.DescEnglish,
        c.Defs.Select(d => BuildValueDefItem(d)),
        c.Items.Where(i => !i.ExcludeFromBase).Select(i => BuildItemItem(i))
    );

    private static IPValueDefItem BuildValueDefItem(ValueDef d) => new ValueDefItem(
        d.Id,
        d.Type, d.Description, d.DescEnglish
    );

    private static IPItemItem BuildItemItem(Item i) => new ItemItem(
        i.Id,
        i.Description, i.DescEnglish,
        i.Percent,
        i.Values.Select(v => BuildValueItem(v)),
        i.Costs.Select(c => BuildCostItem(c)),
        i.Invoices.Select(i => BuildInvoiceItem(i))
    );

    private static IPValueItem BuildValueItem(Value v) => new ValueItem(
        v.Id,
        v.ProdValue, v.TextValue, v.TextEnglish
    );

    private static IPCostItem BuildCostItem(Cost c) => new CostItem(
        c.Id,
        c.CostValue, c.Supplier, c.Text
    );

    private static IPInvoiceItem BuildInvoiceItem(Invoice i) => new InvoiceItem(
        i.Id,
        i.InvoicedValue, i.InvoiceNumber, i.Supplier, i.Text
    );

    private static IRefItem? BuildManagerItem(User? user) => user is User _user ? new RefItem(
        _user.Id,
        _user.FullName
    ) : null;

    private record struct BudgetItem(
        Guid Id, string Code,
        DateOnly BudgetDate, string Title,
        string? FilmDate, string? FinalClient, string? Product, string? Agency,
        string? Director, string? Producer, string? TVAgency, string? Rights,
        string? StudioDays, string? LocationDays, string? OutsideDays, string? WeekendHolidays,
        string? PostProdDuration, string? PostProdVersions, string? PostProdSound, string? PostProdNVoices,
        string? Comments, string? CommentsEnglish,
        int? NWeatherDays, decimal? WeatherTotal, decimal? WeatherPercent,
        IEnumerable<IPGroupItem> Groups,
        string? Owner, IRefItem? Manager
    ) : IProductionDetailsItem;

    private record struct GroupItem(
        Guid Id,
        string Description, string? DescEnglish,
        IEnumerable<IPCategoryItem> Categories
    ) : IPGroupItem;

    private record struct CategoryItem(
        Guid Id,
        string Formula, string Description, string? DescEnglish,
        IEnumerable<IPValueDefItem> Defs,
        IEnumerable<IPItemItem> Items
    ) : IPCategoryItem;

    private record struct ValueDefItem(
        Guid Id,
        ValueType Type, string Description, string? DescEnglish
    ) : IPValueDefItem;

    private record struct ItemItem(
        Guid Id,
        string Description, string? DescEnglish, decimal? Percent,
        IEnumerable<IPValueItem> Values,
        IEnumerable<IPCostItem> Costs, IEnumerable<IPInvoiceItem> Invoices
    ) : IPItemItem;

    private record struct ValueItem(
        Guid Id,
        decimal? NumberValue, string? TextValue, string? TextEnglish
    ) : IPValueItem;

    private record struct CostItem(
        Guid Id,
        decimal CostValue, string? Supplier, string? Text
    ) : IPCostItem;

    private record struct InvoiceItem(
        Guid Id,
        decimal InvoicedValue, string? InvoiceNumber, string? Supplier, string? Text
    ) : IPInvoiceItem;

    private record struct RefItem(
        Guid Id,
        string Description
    ) : IRefItem;
}

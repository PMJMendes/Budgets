using Krypton.Budgets.Blazor.APIClient.Budgets.Budget.BudgetDetails;
using Krypton.Budgets.Blazor.APIClient.Budgets.Budget.ManageBudget;
using Krypton.Budgets.Blazor.APIClient.Budgets.Budget.ProductionDetails;
using Krypton.Budgets.Blazor.APIClient.Budgets.Budget.UpdateBudget;
using Krypton.Budgets.Shared;

namespace Krypton.Budgets.Blazor.UI.Budgets.BudgetPage._Core;

public class ItemModel : IItem
{
    private readonly CategoryModel _owner;

    public ItemModel(Guid id, CategoryModel owner, bool excludeFromBase, bool canBePercent, string description,
        string? descEnglish, decimal? percent, decimal? bCAPercent,
        IEnumerable<ValueModel> values, IEnumerable<CostModel> costs, IEnumerable<InvoiceModel> invoices)
    {
        Id = id;
        _owner = owner;

        ExcludeFromBase = excludeFromBase;
        CanBePercent = canBePercent;
        Description = description;
        DescEnglish = descEnglish;
        Percent = percent;
        BCAPercent = bCAPercent;

        Values = values.Any() ?
            values.Select((v, i) => v.WithDef(owner.Defs.FirstOrDefault(d => d.Order == i + 1) ?? ValueDefModel.Empty()).WithOwner(this)).ToList() :
            owner.Defs.Select(d => ValueModel.Empty().WithDef(d).WithOwner(this)).ToList();
        Costs = costs.ToList();
        Invoices = invoices.ToList();
    }

    public Guid Id { get; set; }

    public bool ExcludeFromBase { get; set; }
    public bool CanBePercent { get; set; }
    public string Description { get; set; }
    public string? DescEnglish { get; set; }
    public decimal? Percent { get; set; }
    public decimal? BCAPercent { get; set; }

    public decimal Value { get; private set; }
    public decimal BCAValue { get; private set; }
    public decimal ClientValue { get; private set; }
    public decimal ProdValue { get; private set; }

    public List<ValueModel> Values { get; private set; }
    public List<CostModel> Costs { get; private set; }
    public List<InvoiceModel> Invoices { get; private set; }

    public ValueModel? this[string index] => Values.FirstOrDefault(v => v.Def.Description == index);

    ICategory IItem.Owner => _owner;
    bool IItem._HasErrors { get; set; }
    IEnumerable<IValue> IItem.Values => Values;
    decimal IItem.Value { get => Value; set => Value = value; }
    decimal IItem.BCAValue { get => BCAValue; set => BCAValue = value; }
    decimal IItem.ClientValue { get => ClientValue; set => ClientValue = value; }
    decimal IItem.ProdValue { get => ProdValue; set => ProdValue = value; }
    IEnumerable<ICost> IItem.Costs => Costs;
    IEnumerable<IInvoice> IItem.Invoices => Invoices;

    public ItemArgs AsUpdateArgs() => new(
        Id,
        ExcludeFromBase,
        CanBePercent,
        Description,
        DescEnglish,
        Percent,
        BCAPercent,
        Values.OrderBy(v => v.Def.Order).Select(v => v.AsArgs()).ToList()
    );

    public MItemArgs AsManageArgs() => new(
        Id,
        Costs.Select(c => c.AsManageArgs()).ToList(),
        Invoices.Select(i => i.AsManageArgs()).ToList()
    );

    public ItemModel WithOwner(CategoryModel owner) => new(
        Id,
        owner,
        ExcludeFromBase,
        CanBePercent,
        Description,
        DescEnglish,
        Percent,
        BCAPercent,
        Values,
        Costs,
        Invoices
    );

    public void AddValue(ValueDefModel def) => Values.Add(ValueModel.Empty().WithDef(def).WithOwner(this));

    public void RemoveValue(ValueDefModel def) => Values.RemoveAll(v => v.Def == def);

    public void AddCost(CostModel cost) => Costs.Add(cost);

    public void RemoveCost(CostModel cost) => Costs.Remove(cost);

    public void AddInvoice(InvoiceModel invoice) => Invoices.Add(invoice);

    public void RemoveInvoice(InvoiceModel invoice) => Invoices.Remove(invoice);

    public ItemModel WithManageResults(PItemItem? item) => new(
        Id,
        _owner,
        ExcludeFromBase,
        CanBePercent,
        Description,
        DescEnglish,
        Percent,
        BCAPercent,
        Values,
        item?.Costs?.Select(c => CostModel.FromService(c)) ?? Array.Empty<CostModel>(),
        item?.Invoices?.Select(i => InvoiceModel.FromService(i)) ?? Array.Empty<InvoiceModel>()
    );

    public ItemModel Clone() => new(
        Id,
        _owner,
        ExcludeFromBase,
        CanBePercent,
        Description,
        DescEnglish,
        Percent,
        BCAPercent,
        Values.Select(v => v.Clone()),
        Costs.Select(c => c.Clone()),
        Invoices.Select(i => i.Clone())
    );

    public static ItemModel Empty() => new(
        Guid.Empty,
        CategoryModel.Empty(),
        false,
        false,
        string.Empty,
        null,
        null,
        null,
        Array.Empty<ValueModel>(),
        Array.Empty<CostModel>(),
        Array.Empty<InvoiceModel>()
    );

    public static ItemModel FromService(ItemItem? item) => new(
        item?.Id ?? Guid.Empty,
        CategoryModel.Empty(),
        item?.ExcludeFromBase ?? false,
        item?.CanBePercent ?? false,
        item?.Description ?? string.Empty,
        item?.DescEnglish,
        item?.Percent,
        item?.BCAPercent,
        item?.Values?.Select(v => ValueModel.FromService(v)) ?? Array.Empty<ValueModel>(),
        item?.Costs?.Select(c => CostModel.FromService(c)) ?? Array.Empty<CostModel>(),
        item?.Invoices?.Select(i => InvoiceModel.FromService(i)) ?? Array.Empty<InvoiceModel>()
    );

    public static ItemModel FromService(PItemItem? item) => new(
        item?.Id ?? Guid.Empty,
        CategoryModel.Empty(),
        false,
        false,
        item?.Description ?? string.Empty,
        item?.DescEnglish,
        item?.Percent,
        null,
        item?.Values?.Select(v => ValueModel.FromService(v)) ?? Array.Empty<ValueModel>(),
        item?.Costs?.Select(c => CostModel.FromService(c)) ?? Array.Empty<CostModel>(),
        item?.Invoices?.Select(i => InvoiceModel.FromService(i)) ?? Array.Empty<InvoiceModel>()
    );
}

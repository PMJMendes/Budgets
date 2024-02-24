using Krypton.Budgets.Domain._Base.Entities;
using Krypton.Budgets.Domain._Impl.Attributes;
using Krypton.Budgets.Domain._Impl.Exceptions;
using Krypton.Budgets.Domain._Impl.Interfaces;
using Krypton.Budgets.Domain._Impl.Utils;
using Krypton.Budgets.Domain.Budgets.Category_;
using Krypton.Budgets.Domain.Budgets.Cost_;
using Krypton.Budgets.Domain.Budgets.Invoice_;
using Krypton.Budgets.Domain.Budgets.Value_;
using Krypton.Budgets.Domain.Budgets.ValueDef_;
using Krypton.Budgets.Shared;
using System.Collections.Immutable;

namespace Krypton.Budgets.Domain.Budgets.Item_;

internal class Item : BaseEntity, IItem
{
    private enum EntityErrors
    {
        VALUES_WRONG_LENGTH
    }

    private readonly ISet<Value> values = new HashSet<Value>();
    private readonly ISet<Cost> costs = new HashSet<Cost>();
    private readonly ISet<Invoice> invoices = new HashSet<Invoice>();

    protected Item() { }

    protected Item(IContext context) : base(context) { }

    public Item(IContext context, Category owner, int order,
        ItemDefData def, ItemData? data = null) : this(context)
    {
        Owner = owner;
        Order = order;

        UpdateDefinition(def);
        foreach (var vDef in Owner.Defs)
        {
            CreateValue(vDef, ValueData.Empty);
        }

        if (data is ItemData _data)
        {
            UpdateDataAsync(_data).GetAwaiter().GetResult();
        }

        Value = 0m;
        ClientValue = 0m;
        ProdValue = 0;

        context.Persistence.PersistAsync(this).GetAwaiter().GetResult();
    }

    [Required]
    public virtual Category Owner { get; protected set; } = default!;
    [External]
    ICategory IItem.Owner => Owner;

    [Required]
    public virtual int Order { get; protected set; } = 0;

    [Required]
    public virtual bool ExcludeFromBase { get; protected set; } = false;

    [Required]
    public virtual bool CanBePercent { get; protected set; } = false;

    public virtual string? Description { get; protected set; } = "";

    public virtual string? DescEnglish { get; protected set; }

    public virtual decimal? Percent { get; protected set; }

    public virtual decimal? BCAPercent { get; protected set; }

    public virtual IEnumerable<Value> Values => values.OrderBy(v => v.Def.Order).ToImmutableList();
    [External]
    IEnumerable<IValue> IItem.Values => values;

    public virtual IEnumerable<Cost> Costs => costs.OrderBy(v => v.Order).ToImmutableList();
    [External]
    IEnumerable<ICost> IItem.Costs => costs;

    public virtual IEnumerable<Invoice> Invoices => invoices.OrderBy(v => v.Order).ToImmutableList();
    [External]
    IEnumerable<IInvoice> IItem.Invoices => invoices;

    [External]
    public virtual decimal Value { get; private set; }
    [External]
    decimal IItem.Value
    {
        get => Value;
        set => Value = value;
    }

    [External]
    public virtual decimal BCAValue { get; private set; }
    [External]
    decimal IItem.BCAValue
    {
        get => BCAValue;
        set => BCAValue = value;
    }

    [External]
    public virtual decimal ClientValue { get; private set; }
    [External]
    decimal IItem.ClientValue
    {
        get => ClientValue;
        set => ClientValue = value;
    }

    [External]
    public virtual decimal ProdValue { get; private set; }
    [External]
    decimal IItem.ProdValue
    {
        get => ProdValue;
        set => ProdValue = value;
    }

    [External]
    bool IItem._HasErrors { get; set; }

    public Value? ValueByDef(IValueDef def) => values.FirstOrDefault(v => v.Def == def);

    public void ReRank(int i) => Order = i;

    public async Task<Item> CloneAsync(Category newOwner, Dictionary<int, ValueDef> defs)
    {
        var result = new Item(context)
        {
            Owner = newOwner,

            Order = Order,
            ExcludeFromBase = ExcludeFromBase,
            CanBePercent = CanBePercent,
            Description = Description,
            DescEnglish = DescEnglish,

            Percent = Percent,
            BCAPercent = BCAPercent
        };

        await context.Persistence.PersistAsync(result);

        result.values.UnionWith(await Task.WhenAll(values.Select(v => v.CloneAsync(result, defs[v.Def.Order]))));

        return result;
    }

    public virtual void UpdateDefinition(ItemDefData data)
    {
        ExcludeFromBase = data.ExcludeFromBase;
        CanBePercent = data.CanBePercent;
        Description = data.Description;
        DescEnglish = data.DescEnglish;
    }

    public virtual async Task UpdateDataAsync(ItemData data)
    {
        if (data.ValueData.Count() != values.Count)
        {
            throw new InvalidArgsException(TypeHelper.GetProperty((Item i) => i.Values),
                EntityErrors.VALUES_WRONG_LENGTH, "Update Data: list of values is the wrong length");
        }

        Percent = CanBePercent ? data.Percent : null;
        BCAPercent = data.BCAPercent;

        var valueBuffer = values.ToDictionary(v => v.Def.Order);
        foreach (var kv in data.ValueData.Select((v, i) => (i, v)))
        {
            valueBuffer[kv.i + 1].UpdateData(kv.v);
        }

        await UpdateCostsAsync(data.CostData);
    }

    public virtual async Task UpdateManagementAsync(ItemData data)
    {
        await UpdateCostsAsync(data.CostData);
        await UpdateInvoicesAsync(data.InvoiceData);
    }

    public virtual Value CreateValue(ValueDef def, ValueData data)
    {
        var value = new Value(context, this, def, data);

        values.Add(value);

        return value;
    }

    public void RemoveValue(Value value)
    {
        values.Remove(value);
    }

    public virtual Cost CreateCost(CostData data)
    {
        var cost = new Cost(context, this, costs.Count + 1, data);

        costs.Add(cost);

        return cost;
    }

    public void RemoveCost(Cost cost)
    {
        costs.Remove(cost);
    }

    public virtual Invoice CreateInvoice(InvoiceData data)
    {
        var invoice = new Invoice(context, this, costs.Count + 1, data);

        invoices.Add(invoice);

        return invoice;
    }

    public void RemoveInvoice(Invoice invoice)
    {
        invoices.Remove(invoice);
    }

    public virtual async Task DeleteAsync()
    {
        foreach (var value in Values)
        {
            await value.DeleteAsync();
        }

        foreach (var cost in Costs)
        {
            await cost.DeleteAsync();
        }

        foreach (var invoice in Invoices)
        {
            await invoice.DeleteAsync();
        }

        Owner.RemoveItem(this);

        await context.Persistence.DeleteAsync(this);
    }

    private async Task UpdateCostsAsync(IEnumerable<CostData> costData)
    {
        var costIds = costData.Select(c => c.Id).ToHashSet();
        var toDelete = costs.Where(c => !costIds.Contains(c.Id)).ToList();
        await Task.WhenAll(toDelete.Select(c => c.DeleteAsync()));

        var costBuffer = costs.ToDictionary(c => c.Id);
        foreach (var kv in costData.Select((v, i) => (i, v)))
        {
            if (costBuffer.TryGetValue(kv.v.Id, out var cost))
            {
                cost.ReRank(kv.i + 1);
                cost.UpdateData(kv.v);
            }
            else
            {
                CreateCost(kv.v).ReRank(kv.i + 1);
            }
        }
    }

    private async Task UpdateInvoicesAsync(IEnumerable<InvoiceData> invoiceData)
    {
        var invoiceIds = invoiceData.Select(i => i.Id).ToHashSet();
        var toDelete = invoices.Where(i => !invoiceIds.Contains(i.Id)).ToList();
        await Task.WhenAll(toDelete.Select(i => i.DeleteAsync()));

        var invoiceBuffer = invoices.ToDictionary(c => c.Id);
        foreach (var kv in invoiceData.Select((v, i) => (i, v)))
        {
            if (invoiceBuffer.TryGetValue(kv.v.Id, out var cost))
            {
                cost.ReRank(kv.i + 1);
                cost.UpdateData(kv.v);
            }
            else
            {
                CreateInvoice(kv.v).ReRank(kv.i + 1);
            }
        }
    }
}

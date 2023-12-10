using Krypton.Budgets.Domain._Base.Entities;
using Krypton.Budgets.Domain._Impl.Attributes;
using Krypton.Budgets.Domain._Impl.Exceptions;
using Krypton.Budgets.Domain._Impl.Interfaces;
using Krypton.Budgets.Domain._Impl.Utils;
using Krypton.Budgets.Domain.Budgets.Group_;
using Krypton.Budgets.Domain.Budgets.Item_;
using Krypton.Budgets.Domain.Budgets.Value_;
using Krypton.Budgets.Domain.Budgets.ValueDef_;
using Krypton.Budgets.Shared;
using System.Collections.Immutable;

namespace Krypton.Budgets.Domain.Budgets.Category_;

internal class Category : BaseEntity, ICategory
{
    private enum EntityErrors
    {
        ITEMS_WRONG_LENGTH
    }

    private readonly ISet<ValueDef> defs = new HashSet<ValueDef>();
    private readonly ISet<Item> items = new HashSet<Item>();

    protected Category() { }

    protected Category(IContext context) : base(context) { }

    public Category(IContext context, Group owner, int order, CategoryDefData def, CategoryData? data = null) : this(context)
    {
        Owner = owner;
        Order = order;

        UpdateDefinitionAsync(def).GetAwaiter().GetResult();

        if (data is CategoryData _data)
        {
            UpdateData(_data);
        }

        context.Persistence.PersistAsync(this).GetAwaiter().GetResult();
    }

    [Required]
    [Unique(Combinations = "businessKey, byGroup")]
    public virtual Group Owner { get; protected set; } = default!;

    [Required]
    [Unique(Combinations = "businessKey")]
    public virtual int Order { get; protected set; } = 0;

    [Required]
    public virtual string Formula { get; protected set; } = "";

    [Required]
    [Unique(Combinations = "byGroup")]
    public virtual string Description { get; protected set; } = "";

    public virtual string? DescEnglish { get; protected set; }

    public virtual IEnumerable<ValueDef> Defs => defs.OrderBy(i => i.Order).ToImmutableList();

    public virtual IEnumerable<Item> Items => items.OrderBy(i => i.Order).ToImmutableList();
    [External]
    IEnumerable<IItem> ICategory.Items => items;

    public void ReRank(int i) => Order = i;

    public async Task<Category> CloneAsync(Group newOwner)
    {
        var result = new Category(context)
        {
            Owner = newOwner,

            Order = Order,
            Formula = Formula,
            Description = Description,
            DescEnglish = DescEnglish,
        };

        await context.Persistence.PersistAsync(result);

        result.defs.UnionWith(await Task.WhenAll(defs.Select(d => d.CloneAsync(result))));
        result.items.UnionWith(await Task.WhenAll(items.Select(i => i.CloneAsync(result, result.defs.ToDictionary(d => d.Order)))));

        return result;
    }

    public virtual async Task UpdateDefinitionAsync(CategoryDefData def)
    {
        Formula = def.Formula;
        Description = def.Description;
        DescEnglish = def.DescEnglish;

        await UpdateValueDefsAsync(def.ValueDefData);
        await UpdateItemDefsAsync(def.ItemDefData);
    }

    public virtual void UpdateData(CategoryData data)
    {
        if (data.ItemData.Count() != items.Count)
        {
            throw new InvalidArgsException(TypeHelper.GetProperty((Category i) => i.Items),
                EntityErrors.ITEMS_WRONG_LENGTH, "Update Items: list of items is the wrong length");
        }

        var itemBuffer = items.ToDictionary(v => v.Order);
        foreach (var kv in data.ItemData.Select((v, i) => (i, v)))
        {
            itemBuffer[kv.i + 1].UpdateData(kv.v);
        }
    }

    public virtual async Task UpdateManagementAsync(CategoryData data)
    {
        if (data.ItemData.Count() != items.Count)
        {
            throw new InvalidArgsException(TypeHelper.GetProperty((Category i) => i.Items),
                EntityErrors.ITEMS_WRONG_LENGTH, "Update Items: list of items is the wrong length");
        }

        var itemBuffer = items.ToDictionary(v => v.Order);
        foreach (var kv in data.ItemData.Select((v, i) => (i, v)))
        {
            await itemBuffer[kv.i + 1].UpdateManagementAsync(kv.v);
        }
    }

    public async Task DeleteValuesAndRemoveValueDefAsync(ValueDef def)
    {
        await Task.WhenAll(items.
            Select(i => i.ValueByDef(def)?.DeleteAsync() ?? Task.CompletedTask)
        );

        defs.Remove(def);
    }

    public void RemoveItem(Item item)
    {
        items.Remove(item);
    }

    public virtual async Task DeleteAsync()
    {
        foreach (var item in Items)
        {
            await item.DeleteAsync();
        }

        foreach (var def in Defs)
        {
            await def.DeleteAsync();
        }

        Owner.RemoveCategory(this);

        await context.Persistence.DeleteAsync(this);
    }

    private async Task UpdateValueDefsAsync(IEnumerable<ValueDefData> valueDefData)
    {
        var newDefs = valueDefData.Select(v => v.Id).ToHashSet();
        var toDelete = defs.Where(v => !newDefs.Contains(v.Id)).ToList();
        await Task.WhenAll(toDelete.Select(v => v.DeleteAsync()));

        var defBuffer = defs.ToDictionary(v => v.Id);
        foreach (var kv in valueDefData.Select((v, i) => (i, v)))
        {
            if (defBuffer.TryGetValue(kv.v.Id, out var value))
            {
                value.ReRank(kv.i + 1);
                value.UpdateDefinition(kv.v);
            }
            else
            {
                CreateValueDef(kv.v).ReRank(kv.i + 1);
            }
        }
    }

    private async Task UpdateItemDefsAsync(IEnumerable<ItemDefData> itemDefData)
    {
        var newItems = itemDefData.Select(i => i.Id).ToHashSet();
        var toDelete = items.Where(i => !newItems.Contains(i.Id)).ToList();
        await Task.WhenAll(toDelete.Select(i => i.DeleteAsync()));

        var itemBuffer = items.ToDictionary(v => v.Id);
        foreach (var kv in itemDefData.Select((v, i) => (i, v)))
        {
            if (itemBuffer.TryGetValue(kv.v.Id, out var item))
            {
                item.ReRank(kv.i + 1);
                item.UpdateDefinition(kv.v);
            }
            else
            {
                CreateItem(kv.v).ReRank(kv.i + 1);
            }
        }
    }

    private ValueDef CreateValueDef(ValueDefData vDef)
    {
        var def = new ValueDef(context, this, defs.Count + 1, vDef);
        foreach (var item in items)
        {
            item.CreateValue(def, ValueData.Empty);
        }

        defs.Add(def);

        return def;
    }

    private Item CreateItem(ItemDefData def)
    {
        var item = new Item(context, this, items.Count + 1, def);

        items.Add(item);

        return item;
    }
}

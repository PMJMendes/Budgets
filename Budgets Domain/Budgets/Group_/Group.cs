using Krypton.Budgets.Domain._Base.Entities;
using Krypton.Budgets.Domain._Impl.Attributes;
using Krypton.Budgets.Domain._Impl.Exceptions;
using Krypton.Budgets.Domain._Impl.Interfaces;
using Krypton.Budgets.Domain._Impl.Utils;
using Krypton.Budgets.Domain.Budgets.Budget_;
using Krypton.Budgets.Domain.Budgets.Category_;
using Krypton.Budgets.Shared;
using System.Collections.Immutable;

namespace Krypton.Budgets.Domain.Budgets.Group_;

internal class Group : BaseEntity, IGroup
{
    private enum EntityErrors
    {
        CATEGORIES_WRONG_LENGTH,
    }

    private readonly ISet<Category> categories = new HashSet<Category>();

    protected Group() { }

    protected Group(IContext context) : base(context) { }

    public Group(IContext context, Budget owner, int order, GroupDefData def, GroupData? data = null) : this(context)
    {
        Owner = owner;
        Order = order;

        UpdateDefinitionAsync(def).GetAwaiter().GetResult();

        if (data is GroupData _data)
        {
            UpdateDataAsync(_data).GetAwaiter().GetResult();
        }

        context.Persistence.PersistAsync(this).GetAwaiter().GetResult();
    }

    [Required]
    public virtual Budget Owner { get; protected set; } = default!;

    [Required]
    public virtual int Order { get; protected set; } = 0;

    public virtual string? Description { get; protected set; }

    public virtual string? DescEnglish { get; protected set; }

    public virtual IEnumerable<Category> Categories => categories.OrderBy(c => c.Order).ToImmutableList();
    [External]
    IEnumerable<ICategory> IGroup.Categories => categories;

    public void ReRank(int i) => Order = i;

    public async Task<Group> CloneAsync(Budget newOwner)
    {
        var result = new Group(context)
        {
            Owner = newOwner,

            Order = Order,
            Description = Description,
            DescEnglish = DescEnglish
        };

        await context.Persistence.PersistAsync(result);

        result.categories.UnionWith(await Task.WhenAll(categories.Select(c => c.CloneAsync(result))));

        return result;
    }

    public virtual async Task UpdateDefinitionAsync(GroupDefData def)
    {
        Description = def.Description;
        DescEnglish = def.DescEnglish;

        await UpdateCategoryDefsAsync(def.CategoryDefData);
    }

    public virtual async Task UpdateDataAsync(GroupData data)
    {
        if (data.CategoryData.Count() != categories.Count)
        {
            throw new InvalidArgsException(TypeHelper.GetProperty((Group i) => i.Categories),
                EntityErrors.CATEGORIES_WRONG_LENGTH, "Update Categories: list of categories is the wrong length");
        }

        var categoryBuffer = categories.ToDictionary(v => v.Order);
        foreach (var kv in data.CategoryData.Select((v, i) => (i, v)))
        {
            await categoryBuffer[kv.i + 1].UpdateDataAsync(kv.v);
        }
    }

    public virtual async Task UpdateManagementAsync(GroupData data)
    {
        if (data.CategoryData.Count() != categories.Count)
        {
            throw new InvalidArgsException(TypeHelper.GetProperty((Group i) => i.Categories),
                EntityErrors.CATEGORIES_WRONG_LENGTH, "Update Categories: list of categories is the wrong length");
        }

        var categoryBuffer = categories.ToDictionary(v => v.Order);
        foreach (var kv in data.CategoryData.Select((v, i) => (i, v)))
        {
            await categoryBuffer[kv.i + 1].UpdateManagementAsync(kv.v);
        }
    }

    public void RemoveCategory(Category category)
    {
        categories.Remove(category);
    }

    public virtual async Task DeleteAsync()
    {
        foreach (var category in Categories)
        {
            await category.DeleteAsync();
        }

        Owner.RemoveGroup(this);

        await context.Persistence.DeleteAsync(this);
    }

    private async Task UpdateCategoryDefsAsync(IEnumerable<CategoryDefData> categoryDefData)
    {
        var categoryIds = categoryDefData.Select(c => c.Id).ToHashSet();
        var toDelete = categories.Where(i => !categoryIds.Contains(i.Id)).ToList();
        await Task.WhenAll(toDelete.Select(i => i.DeleteAsync()));

        var categoryBuffer = categories.ToDictionary(v => v.Id);
        foreach (var kv in categoryDefData.Select((v, i) => (i, v)))
        {
            if (categoryBuffer.TryGetValue(kv.v.Id, out var category))
            {
                category.ReRank(kv.i + 1);
                await category.UpdateDefinitionAsync(kv.v);
            }
            else
            {
                CreateCategory(kv.v).ReRank(kv.i + 1);
            }
        }
    }

    private Category CreateCategory(CategoryDefData def)
    {
        var category = new Category(context, this, categories.Count + 1, def);

        categories.Add(category);

        return category;
    }
}

using Krypton.Budgets.Domain.Budgets.Budget_.BudgetDetails;
using System.Text.Json.Serialization;

namespace Krypton.Budgets.API.Budgets.Budget.BudgetDetails;

internal readonly struct CategoryItem : ICategoryItem
{
    public CategoryItem(ICategoryItem source)
    {
        Id = source.Id;
        Formula = source.Formula;
        Description = source.Description;
        DescEnglish = source.DescEnglish;
        Defs = source.Defs.Select(d => new ValueDefItem(d));
        Items = source.Items.Select(i => new ItemItem(i));
    }

    public Guid Id { get; private init; }
    public string Formula { get; private init; }
    public string? Description { get; private init; }
    public string? DescEnglish { get; private init; }
    public IEnumerable<ValueDefItem> Defs { get; private init; }
    public IEnumerable<ItemItem> Items { get; private init; }

    [JsonIgnore]
    IEnumerable<IValueDefItem> ICategoryItem.Defs => throw new NotImplementedException();
    [JsonIgnore]
    IEnumerable<IItemItem> ICategoryItem.Items => throw new NotImplementedException();
}

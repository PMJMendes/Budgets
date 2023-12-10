using Krypton.Budgets.Domain.Budgets.Budget_.BudgetDetails;
using System.Text.Json.Serialization;

namespace Krypton.Budgets.API.Budgets.Budget.BudgetDetails;

internal readonly struct GroupItem : IGroupItem
{
    public GroupItem(IGroupItem source)
    {
        Id = source.Id;
        Description = source.Description;
        DescEnglish = source.DescEnglish;
        Categories = source.Categories.Select(c => new CategoryItem(c));
    }

    public Guid Id { get; private init; }
    public string Description { get; private init; }
    public string? DescEnglish { get; private init; }
    public IEnumerable<CategoryItem> Categories { get; private init; }

    [JsonIgnore]
    IEnumerable<ICategoryItem> IGroupItem.Categories => throw new NotImplementedException();
}

using Krypton.Budgets.Domain.Budgets.Budget_.ProductionDetails;
using System.Text.Json.Serialization;

namespace Krypton.Budgets.API.Budgets.Budget.ProductionDetails;

internal readonly struct PGroupItem : IPGroupItem
{
    public PGroupItem(IPGroupItem source)
    {
        Id = source.Id;
        Description = source.Description;
        DescEnglish = source.DescEnglish;
        Categories = source.Categories.Select(c => new PCategoryItem(c));
    }

    public Guid Id { get; private init; }
    public string? Description { get; private init; }
    public string? DescEnglish { get; private init; }
    public IEnumerable<PCategoryItem> Categories { get; private init; }

    [JsonIgnore]
    IEnumerable<IPCategoryItem> IPGroupItem.Categories => throw new NotImplementedException();
}

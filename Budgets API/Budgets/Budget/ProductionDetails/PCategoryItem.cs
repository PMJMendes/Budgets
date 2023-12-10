using Krypton.Budgets.Domain.Budgets.Budget_.ProductionDetails;
using System.Text.Json.Serialization;

namespace Krypton.Budgets.API.Budgets.Budget.ProductionDetails;

internal readonly struct PCategoryItem : IPCategoryItem
{
    public PCategoryItem(IPCategoryItem source)
    {
        Id = source.Id;
        Formula = source.Formula;
        Description = source.Description;
        DescEnglish = source.DescEnglish;
        Defs = source.Defs.Select(d => new PValueDefItem(d));
        Items = source.Items.Select(i => new PItemItem(i));
    }

    public Guid Id { get; private init; }
    public string Formula { get; private init; }
    public string Description { get; private init; }
    public string? DescEnglish { get; private init; }
    public IEnumerable<PValueDefItem> Defs { get; private init; }
    public IEnumerable<PItemItem> Items { get; private init; }

    [JsonIgnore]
    IEnumerable<IPValueDefItem> IPCategoryItem.Defs => throw new NotImplementedException();
    [JsonIgnore]
    IEnumerable<IPItemItem> IPCategoryItem.Items => throw new NotImplementedException();
}

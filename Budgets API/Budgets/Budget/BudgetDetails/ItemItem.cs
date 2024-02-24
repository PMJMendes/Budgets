using Krypton.Budgets.Domain.Budgets.Budget_.BudgetDetails;
using System.Text.Json.Serialization;

namespace Krypton.Budgets.API.Budgets.Budget.BudgetDetails;

internal readonly struct ItemItem : IItemItem
{
    public ItemItem(IItemItem source)
    {
        Id = source.Id;
        ExcludeFromBase = source.ExcludeFromBase;
        CanBePercent = source.CanBePercent;
        Description = source.Description;
        DescEnglish = source.DescEnglish;
        Percent = source.Percent;
        BCAPercent = source.BCAPercent;
        Values = source.Values.Select(v => new ValueItem(v));
        Costs = source.Costs.Select(c => new CostItem(c));
        Invoices = source.Invoices.Select(i => new InvoiceItem(i));
    }

    public Guid Id { get; private init; }
    public bool ExcludeFromBase { get; private init; }
    public bool CanBePercent { get; private init; }
    public string? Description { get; private init; }
    public string? DescEnglish { get; private init; }
    public decimal? Percent { get; private init; }
    public decimal? BCAPercent { get; private init; }
    public IEnumerable<ValueItem> Values { get; private init; }
    public IEnumerable<CostItem> Costs { get; private init; }
    public IEnumerable<InvoiceItem> Invoices { get; private init; }

    [JsonIgnore]
    IEnumerable<IValueItem> IItemItem.Values => throw new NotImplementedException();
    [JsonIgnore]
    IEnumerable<ICostItem> IItemItem.Costs => throw new NotImplementedException();
    [JsonIgnore]
    IEnumerable<IInvoiceItem> IItemItem.Invoices => throw new NotImplementedException();
}

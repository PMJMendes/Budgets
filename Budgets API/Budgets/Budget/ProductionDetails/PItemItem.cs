using Krypton.Budgets.Domain.Budgets.Budget_.ProductionDetails;
using System.Text.Json.Serialization;

namespace Krypton.Budgets.API.Budgets.Budget.ProductionDetails;

internal readonly struct PItemItem : IPItemItem
{
    public PItemItem(IPItemItem source)
    {
        Id = source.Id;
        Description = source.Description;
        DescEnglish = source.DescEnglish;
        Percent = source.Percent;
        Values = source.Values.Select(v => new PValueItem(v));
        Costs = source.Costs.Select(c => new PCostItem(c));
        Invoices = source.Invoices.Select(c => new PInvoiceItem(c));
    }

    public Guid Id { get; private init; }
    public string? Description { get; private init; }
    public string? DescEnglish { get; private init; }
    public decimal? Percent { get; private init; }
    public IEnumerable<PValueItem> Values { get; private init; }
    public IEnumerable<PCostItem> Costs { get; private init; }
    public IEnumerable<PInvoiceItem> Invoices { get; private init; }

    [JsonIgnore]
    IEnumerable<IPValueItem> IPItemItem.Values => throw new NotImplementedException();
    [JsonIgnore]
    IEnumerable<IPCostItem> IPItemItem.Costs => throw new NotImplementedException();
    [JsonIgnore]
    IEnumerable<IPInvoiceItem> IPItemItem.Invoices => throw new NotImplementedException();
}

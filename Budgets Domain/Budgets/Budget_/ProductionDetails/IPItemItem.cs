namespace Krypton.Budgets.Domain.Budgets.Budget_.ProductionDetails;

public interface IPItemItem
{
    Guid Id { get; }

    string? Description { get; }
    string? DescEnglish { get; }
    decimal? Percent { get; }

    IEnumerable<IPValueItem> Values { get; }
    IEnumerable<IPCostItem> Costs { get; }
    IEnumerable<IPInvoiceItem> Invoices { get; }
}

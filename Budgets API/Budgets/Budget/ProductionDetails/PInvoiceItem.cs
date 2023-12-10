using Krypton.Budgets.Domain.Budgets.Budget_.ProductionDetails;

namespace Krypton.Budgets.API.Budgets.Budget.ProductionDetails;

internal readonly struct PInvoiceItem : IPInvoiceItem
{
    public PInvoiceItem(IPInvoiceItem source)
    {
        Id = source.Id;
        InvoicedValue = source.InvoicedValue;
        InvoiceNumber = source.InvoiceNumber;
        Supplier = source.Supplier;
        Text = source.Text;
    }

    public Guid Id { get; private init; }
    public decimal InvoicedValue { get; private init; }
    public string? InvoiceNumber { get; private init; }
    public string? Supplier { get; private init; }
    public string? Text { get; private init; }
}

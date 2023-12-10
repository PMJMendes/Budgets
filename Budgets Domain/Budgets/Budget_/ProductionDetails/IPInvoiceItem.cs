namespace Krypton.Budgets.Domain.Budgets.Budget_.ProductionDetails;

public interface IPInvoiceItem
{
    Guid Id { get; }

    decimal InvoicedValue { get; }
    string? InvoiceNumber { get; }
    string? Supplier { get; }
    string? Text { get; }
}

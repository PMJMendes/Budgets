namespace Krypton.Budgets.Domain.Budgets.Invoice_;

internal record struct InvoiceData(
    Guid Id,
    decimal InvoicedValue,
    string? InvoiceNumber,
    string? Supplier,
    string? Text
);

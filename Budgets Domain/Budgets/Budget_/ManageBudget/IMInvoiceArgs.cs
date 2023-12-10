using Krypton.Budgets.Domain._Impl.Attributes;
using Krypton.Budgets.Domain.Budgets.Invoice_;

namespace Krypton.Budgets.Domain.Budgets.Budget_.ManageBudget;

public interface IMInvoiceArgs
{
    Guid? Id { get; }

    [Required]
    decimal? InvoicedValue { get; }

    string? InvoiceNumber { get; }
    string? Supplier { get; }
    string? Text { get; }

    internal sealed InvoiceData ToInvoiceData() => new(
        Id ?? Guid.Empty,
        InvoicedValue ?? 0m,
        InvoiceNumber,
        Supplier,
        Text
    );
}

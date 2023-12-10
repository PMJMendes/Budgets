namespace Krypton.Budgets.Domain.Budgets.Budget_.BudgetDetails;

public interface IInvoiceItem
{
    Guid Id { get; }

    decimal InvoicedValue { get; }
    string? InvoiceNumber { get; }
    string? Supplier { get; }
    string? Text { get; }
}

using Krypton.Budgets.Domain.Budgets.Budget_.BudgetDetails;

namespace Krypton.Budgets.API.Budgets.Budget.BudgetDetails;

internal readonly struct InvoiceItem : IInvoiceItem
{
    public InvoiceItem(IInvoiceItem source)
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

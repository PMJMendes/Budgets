namespace Krypton.Budgets.Blazor.APIClient.Budgets.Budget.ManageBudget;

public class MInvoiceArgs
{
    public MInvoiceArgs(Guid? id, decimal invoicedValue, string? invoiceNumber, string? supplier, string? text)
    {
        Id = id;
        InvoicedValue = invoicedValue;
        InvoiceNumber = invoiceNumber;
        Supplier = supplier;
        Text = text;
    }

    public Guid? Id { get; private init; }
    public decimal InvoicedValue { get; private init; }
    public string? InvoiceNumber { get; private init; }
    public string? Supplier { get; private init; }
    public string? Text { get; private init; }
}

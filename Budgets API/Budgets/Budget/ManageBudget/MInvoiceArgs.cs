using Krypton.Budgets.Domain.Budgets.Budget_.ManageBudget;

namespace Krypton.Budgets.API.Budgets.Budget.ManageBudget;

internal class MInvoiceArgs : IMInvoiceArgs
{
    public Guid? Id { get; set; }

    public decimal? InvoicedValue { get; set; }
    public string? InvoiceNumber { get; set; }
    public string? Supplier { get; set; }
    public string? Text { get; set; }
}

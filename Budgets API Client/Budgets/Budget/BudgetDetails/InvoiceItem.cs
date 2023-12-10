namespace Krypton.Budgets.Blazor.APIClient.Budgets.Budget.BudgetDetails;

public class InvoiceItem
{
    public Guid? Id { get; set; }
    public decimal? InvoicedValue { get; set; }
    public string? InvoiceNumber { get; set; }
    public string? Supplier { get; set; }
    public string? Text { get; set; }
}

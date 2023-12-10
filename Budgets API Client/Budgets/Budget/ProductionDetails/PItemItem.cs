namespace Krypton.Budgets.Blazor.APIClient.Budgets.Budget.ProductionDetails;

public class PItemItem
{
    public Guid? Id { get; set; }
    public string? Description { get; set; }
    public string? DescEnglish { get; set; }
    public decimal? Percent { get; set; }
    public IEnumerable<PValueItem?>? Values { get; set; }
    public IEnumerable<PCostItem?>? Costs { get; set; }
    public IEnumerable<PInvoiceItem?>? Invoices { get; set; }
}

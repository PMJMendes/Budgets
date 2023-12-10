namespace Krypton.Budgets.Blazor.APIClient.Budgets.Budget.BudgetDetails;

public class ItemItem
{
    public Guid? Id { get; set; }
    public bool? ExcludeFromBase { get; set; }
    public bool? CanBePercent { get; set; }
    public string? Description { get; set; }
    public string? DescEnglish { get; set; }
    public decimal? Percent { get; set; }
    public decimal? BCAPercent { get; set; }
    public IEnumerable<ValueItem?>? Values { get; set; }
    public IEnumerable<CostItem?>? Costs { get; set; }
    public IEnumerable<InvoiceItem?>? Invoices { get; set; }
}

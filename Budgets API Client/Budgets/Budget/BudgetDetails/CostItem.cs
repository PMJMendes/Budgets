namespace Krypton.Budgets.Blazor.APIClient.Budgets.Budget.BudgetDetails;

public class CostItem
{
    public Guid? Id { get; set; }
    public decimal? CostValue { get; set; }
    public string? Supplier { get; set; }
    public string? Text { get; set; }
}

namespace Krypton.Budgets.Blazor.APIClient.Budgets.Budget.BudgetDetails;

public class CategoryItem
{
    public Guid? Id { get; set; }
    public string? Formula { get; set; }
    public string? Description { get; set; }
    public string? DescEnglish { get; set; }
    public IEnumerable<ValueDefItem?>? Defs { get; set; }
    public IEnumerable<ItemItem?>? Items { get; set; }
}

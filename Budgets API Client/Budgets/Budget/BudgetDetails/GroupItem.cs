namespace Krypton.Budgets.Blazor.APIClient.Budgets.Budget.BudgetDetails;

public class GroupItem
{
    public Guid? Id { get; set; }
    public string? Description { get; set; }
    public string? DescEnglish { get; set; }
    public IEnumerable<CategoryItem?>? Categories { get; set; }
}

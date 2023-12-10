namespace Krypton.Budgets.Blazor.APIClient.Budgets.Budget.ProductionDetails;

public class PGroupItem
{
    public Guid? Id { get; set; }
    public string? Description { get; set; }
    public string? DescEnglish { get; set; }
    public IEnumerable<PCategoryItem?>? Categories { get; set; }
}

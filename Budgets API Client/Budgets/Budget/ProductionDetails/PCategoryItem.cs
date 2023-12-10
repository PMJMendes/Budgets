namespace Krypton.Budgets.Blazor.APIClient.Budgets.Budget.ProductionDetails;

public class PCategoryItem
{
    public Guid? Id { get; set; }
    public string? Formula { get; set; }
    public string? Description { get; set; }
    public string? DescEnglish { get; set; }
    public IEnumerable<PValueDefItem?>? Defs { get; set; }
    public IEnumerable<PItemItem?>? Items { get; set; }
}

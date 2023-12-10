namespace Krypton.Budgets.Blazor.APIClient._Common;

public class ErrorResultsItem
{
    public string? Name { get; set; }
    public string? Reason { get; set; }
    public string? Message { get; set; }
    public Dictionary<string, object>? Attributes { get; set; }
}

namespace Krypton.Budgets.Blazor.APIClient._Common;

public class ErrorResults
{
    public static readonly ErrorResults NOT_FOUND = new() { InvalidParams = new ErrorResultsItem[] { new() { Reason = "NOT_FOUND" } } };
    public static readonly ErrorResults UNAUTHORIZED = new() { InvalidParams = new ErrorResultsItem[] { new() { Reason = "UNAUTHORIZED" } } };
    public static readonly ErrorResults FORBIDDEN = new() { InvalidParams = new ErrorResultsItem[] { new() { Reason = "FORBIDDEN" } } };
    public static readonly ErrorResults UNKNOWN_ERROR = new() { InvalidParams = new ErrorResultsItem[] { new() { Reason = "UNKNOWN_ERROR" } } };

    public string? Type { get; set; }
    public int? Status { get; set; }
    public IEnumerable<ErrorResultsItem?>? InvalidParams { get; set; }
}

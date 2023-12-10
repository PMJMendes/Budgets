using Krypton.Budgets.Blazor.APIClient._Base;

namespace Krypton.Budgets.Blazor.APIClient.Global.NextBudgetCode;

public class NextBudgetCodeArgs : IQueryArgs
{
    public NextBudgetCodeArgs(string? prefix)
    {
        Prefix = prefix;
    }

    public string? Prefix { get; private init; }

    public Dictionary<string, string?> GetQueryString() => new()
    {
        [nameof(Prefix)] = Prefix,
    };
}

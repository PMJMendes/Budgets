using Krypton.Budgets.Blazor.APIClient._Base;

namespace Krypton.Budgets.Blazor.APIClient._Common;

public class TargetQueryArgs : IQueryArgs
{
    public TargetQueryArgs(Guid? targetId)
    {
        TargetId = targetId;
    }

    public Guid? TargetId { get; private init; }

    public Dictionary<string, string?> GetQueryString() => new()
    {
        [nameof(TargetId)] = TargetId?.ToString()
    };
}

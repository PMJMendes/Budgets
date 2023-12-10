namespace Krypton.Budgets.Blazor.APIClient._Common;

public class TargetPostArgs
{
    public TargetPostArgs(Guid? targetId)
    {
        TargetId = targetId;
    }

    public Guid? TargetId { get; private init; }
}

using Krypton.Budgets.Blazor.APIClient._Common;

namespace Krypton.Budgets.Blazor.APIClient.Budgets.Budget.AssignManager;

public class AssignManagerArgs : TargetPostArgs
{
    public AssignManagerArgs(Guid? targetId, Guid? newManagerId) : base(targetId)
    {
        NewManagerId = newManagerId;
    }

    public Guid? NewManagerId { get; private init; }
}

using Krypton.Budgets.Blazor.APIClient._Common;

namespace Krypton.Budgets.Blazor.APIClient.Budgets.Budget.UpdateBudget;

public class UpdateBudgetArgs : TargetPostArgs
{
    public UpdateBudgetArgs(Guid targetId, string newCode, FrontDataArgs frontData, FinalDataArgs finalData,
        IEnumerable<GroupArgs> groupArgs) : base(targetId)
    {
        NewCode = newCode;
        FrontData = frontData;
        FinalData = finalData;
        GroupArgs = groupArgs;
    }

    public string NewCode { get; private init; }
    public FrontDataArgs FrontData { get; private init; }
    public FinalDataArgs FinalData { get; private init; }
    public IEnumerable<GroupArgs> GroupArgs { get; private init; }
}

using Krypton.Budgets.Blazor.APIClient._Common;

namespace Krypton.Budgets.Blazor.APIClient.Budgets.Budget.ManageBudget;

public class ManageBudgetArgs : TargetPostArgs
{
    public ManageBudgetArgs(Guid targetId, int? nWeatherDays, IEnumerable<MGroupArgs> groupArgs) : base(targetId)
    {
        NWeatherDays = nWeatherDays;
        GroupArgs = groupArgs;
    }

    public int? NWeatherDays { get; private init; }

    public IEnumerable<MGroupArgs> GroupArgs { get; private init; }
}

using Krypton.Budgets.API._Impl;
using Krypton.Budgets.Domain.Budgets.Budget_.ManageBudget;
using System.Text.Json.Serialization;

namespace Krypton.Budgets.API.Budgets.Budget.ManageBudget;

internal class ManageBudgetArgs : TargetArgs, IManageBudgetArgs
{
    public int? NWeatherDays { get; set; }

    public List<MGroupArgs>? GroupArgs { get; set; }

    [JsonIgnore]
    IEnumerable<IMGroupArgs>? IManageBudgetArgs.GroupArgs => GroupArgs?.Cast<IMGroupArgs>();
}

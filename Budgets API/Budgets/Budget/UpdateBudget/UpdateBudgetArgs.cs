using Krypton.Budgets.API._Impl;
using Krypton.Budgets.Domain.Budgets.Budget_.RecodeBudget;
using Krypton.Budgets.Domain.Budgets.Budget_.UpdateBudget;
using System.Text.Json.Serialization;

namespace Krypton.Budgets.API.Budgets.Budget.UpdateBudget;

internal class UpdateBudgetArgs : TargetArgs, IRecodeBudgetArgs, IUpdateBudgetArgs
{
    public string? NewCode { get; set; }
    public FrontDataArgs? FrontData { get; set; }
    public FinalDataArgs? FinalData { get; set; }
    public List<GroupArgs>? GroupArgs { get; set; }

    [JsonIgnore]
    IFrontDataArgs? IUpdateBudgetArgs.FrontData => FrontData;

    [JsonIgnore]
    IFinalDataArgs? IUpdateBudgetArgs.FinalData => FinalData;
    [JsonIgnore]
    IEnumerable<IGroupArgs>? IUpdateBudgetArgs.GroupArgs => GroupArgs?.Cast<IGroupArgs>();
}

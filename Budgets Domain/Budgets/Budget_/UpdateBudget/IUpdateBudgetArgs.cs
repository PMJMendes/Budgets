using Krypton.Budgets.Domain._Base.Interfaces;
using Krypton.Budgets.Domain._Impl.Attributes;

namespace Krypton.Budgets.Domain.Budgets.Budget_.UpdateBudget;

public interface IUpdateBudgetArgs : ITargetArgs
{
    [Required]
    IFrontDataArgs? FrontData { get; }
    [Required]
    IFinalDataArgs? FinalData { get; }

    [Required]
    IEnumerable<IGroupArgs>? GroupArgs { get; }

    internal sealed BudgetDefData ToDefData(Exception ex) => new(
        GroupArgs?.Select(g => g.ToGroupDef(ex)) ?? throw ex
    );

    internal sealed BudgetData ToBudgetData(Exception ex) => new(
        GroupArgs?.Select(g => g.ToGroupData(ex)) ?? throw ex
    );
}

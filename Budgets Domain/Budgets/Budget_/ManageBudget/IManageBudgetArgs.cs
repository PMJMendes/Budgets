using Krypton.Budgets.Domain._Base.Interfaces;
using Krypton.Budgets.Domain._Impl.Attributes;

namespace Krypton.Budgets.Domain.Budgets.Budget_.ManageBudget;

public interface IManageBudgetArgs : ITargetArgs
{
    int? NWeatherDays { get; }

    [Required]
    IEnumerable<IMGroupArgs>? GroupArgs { get; }

    internal sealed BudgetData ToBudgetData(Exception ex) => new(
        GroupArgs?.Select(g => g.ToGroupData(ex)) ?? throw ex
    );
}

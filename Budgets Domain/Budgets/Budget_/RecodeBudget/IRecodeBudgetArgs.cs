using Krypton.Budgets.Domain._Base.Interfaces;
using Krypton.Budgets.Domain._Impl.Attributes;

namespace Krypton.Budgets.Domain.Budgets.Budget_.RecodeBudget;

public interface IRecodeBudgetArgs : ITargetArgs
{
    [Required]
    string? NewCode { get; }
}

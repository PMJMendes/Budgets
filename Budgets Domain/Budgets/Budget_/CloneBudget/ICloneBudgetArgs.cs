using Krypton.Budgets.Domain._Base.Interfaces;
using Krypton.Budgets.Domain._Impl.Attributes;

namespace Krypton.Budgets.Domain.Budgets.Budget_.CloneBudget;

public interface ICloneBudgetArgs : ITargetArgs
{
    [Required]
    string? NewCode { get; }

    [Required]
    bool? AsTemplate { get; }

    [Required]
    DateOnly? NewBudgetDate { get; }

    string? NewTitle { get; }
}

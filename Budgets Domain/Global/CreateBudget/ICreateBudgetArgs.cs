using Krypton.Budgets.Domain._Base.Interfaces;
using Krypton.Budgets.Domain._Impl.Attributes;

namespace Krypton.Budgets.Domain.Global.CreateBudget;

public interface ICreateBudgetArgs : IArguments
{
    [Required]
    string? Code { get; }

    [Required]
    bool? AsTemplate { get; }

    [Required]
    DateOnly? BudgetDate { get; }

    [Required]
    string? Title { get; }
}

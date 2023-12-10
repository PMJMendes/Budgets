using Krypton.Budgets.API._Impl;
using Krypton.Budgets.Domain.Budgets.Budget_.CloneBudget;

namespace Krypton.Budgets.API.Budgets.Budget.CloneBudget;

internal class CloneBudgetArgs : TargetArgs, ICloneBudgetArgs
{
    public string? NewCode { get; set; }

    public bool? AsTemplate { get; set; }

    public DateOnly? NewBudgetDate { get; set; }

    public string? NewTitle { get; set; }
}

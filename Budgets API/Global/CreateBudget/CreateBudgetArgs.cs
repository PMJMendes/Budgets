using Krypton.Budgets.Domain.Global.CreateBudget;

namespace Krypton.Budgets.API.Global.CreateBudget;

internal class CreateBudgetArgs : ICreateBudgetArgs
{
    public string? Code { get; set; }

    public bool? AsTemplate { get; set; }

    public DateOnly? BudgetDate { get; set; }

    public string? Title { get; set; }
}

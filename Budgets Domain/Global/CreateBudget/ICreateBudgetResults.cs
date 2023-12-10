using Krypton.Budgets.Domain._Base.Interfaces;
using Krypton.Budgets.Domain.Budgets.Budget_;

namespace Krypton.Budgets.Domain.Global.CreateBudget;

public interface ICreateBudgetResults : IOpResults
{
    Guid NewBudgetId { get; }

    bool IsTemplate { get; }

    BudgetState State { get; }
}

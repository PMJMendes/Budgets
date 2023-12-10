using Krypton.Budgets.Domain._Base.Interfaces;
using Krypton.Budgets.Domain.Budgets.Budget_;

namespace Krypton.Budgets.Domain.Global.SearchBudgets;

public interface ISearchBudgetsItem : IQueryResultItem
{
    Guid Id { get; }

    string Code { get; }

    DateOnly BudgetDate { get; }

    string Title { get; }

    BudgetState State { get; }

    string? FinalClient { get; }

    string? Product { get; }
}

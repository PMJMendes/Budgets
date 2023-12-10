using Krypton.Budgets.Domain.Budgets.Budget_;
using Krypton.Budgets.Domain.Global.SearchBudgets;

namespace Krypton.Budgets.API.Global.SearchBudgets;

internal readonly struct SearchBudgetsItem : ISearchBudgetsItem
{
    public SearchBudgetsItem(ISearchBudgetsItem source)
    {
        Id = source.Id;
        Code = source.Code;
        BudgetDate = source.BudgetDate;
        Title = source.Title;
        State = source.State;
        FinalClient = source.FinalClient;
        Product = source.Product;
    }

    public Guid Id { get; private init; }
    public string Code { get; private init; }
    public DateOnly BudgetDate { get; private init; }
    public string Title { get; private init; }
    public BudgetState State { get; private init; }
    public string? FinalClient { get; private init; }
    public string? Product { get; private init; }
}

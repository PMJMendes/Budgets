using Krypton.Budgets.Domain.Budgets.Budget_;
using Krypton.Budgets.Domain.Global.CreateBudget;

namespace Krypton.Budgets.API.Global.CreateBudget;

internal readonly struct CreateBudgetResults : ICreateBudgetResults
{
    public CreateBudgetResults(ICreateBudgetResults source)
    {
        NewBudgetId = source.NewBudgetId;
        IsTemplate = source.IsTemplate;
        State = source.State;
    }

    public Guid NewBudgetId { get; private init; }
    public bool IsTemplate { get; private init; }
    public BudgetState State { get; private init; }
}

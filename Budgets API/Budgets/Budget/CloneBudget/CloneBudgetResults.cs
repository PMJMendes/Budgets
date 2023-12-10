using Krypton.Budgets.Domain.Budgets.Budget_;
using Krypton.Budgets.Domain.Budgets.Budget_.CloneBudget;

namespace Krypton.Budgets.API.Budgets.Budget.CloneBudget;

internal readonly struct CloneBudgetResults : ICloneBudgetResults
{
    public CloneBudgetResults(ICloneBudgetResults source)
    {
        NewBudgetId = source.NewBudgetId;
        IsTemplate = source.IsTemplate;
        State = source.State;
    }

    public Guid NewBudgetId { get; private init; }
    public bool IsTemplate { get; private init; }
    public BudgetState State { get; private init; }
}

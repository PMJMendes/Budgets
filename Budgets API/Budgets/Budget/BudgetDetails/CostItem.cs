using Krypton.Budgets.Domain.Budgets.Budget_.BudgetDetails;

namespace Krypton.Budgets.API.Budgets.Budget.BudgetDetails;

internal readonly struct CostItem : ICostItem
{
    public CostItem(ICostItem source)
    {
        Id = source.Id;
        CostValue = source.CostValue;
        Supplier = source.Supplier;
        Text = source.Text;
    }

    public Guid Id { get; private init; }
    public decimal CostValue { get; private init; }
    public string? Supplier { get; private init; }
    public string? Text { get; private init; }
}

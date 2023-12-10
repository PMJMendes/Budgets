using Krypton.Budgets.Domain.Budgets.Budget_.ProductionDetails;

namespace Krypton.Budgets.API.Budgets.Budget.ProductionDetails;

internal readonly struct PCostItem : IPCostItem
{
    public PCostItem(IPCostItem source)
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

namespace Krypton.Budgets.Domain.Budgets.Budget_.ProductionDetails;

public interface IPCostItem
{
    Guid Id { get; }

    decimal CostValue { get; }
    string? Supplier { get; }
    string? Text { get; }
}

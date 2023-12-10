namespace Krypton.Budgets.Domain.Budgets.Budget_.BudgetDetails;

public interface ICostItem
{
    Guid Id { get; }

    decimal CostValue { get; }
    string? Supplier { get; }
    string? Text { get; }
}

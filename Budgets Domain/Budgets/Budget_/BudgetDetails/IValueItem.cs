namespace Krypton.Budgets.Domain.Budgets.Budget_.BudgetDetails;

public interface IValueItem
{
    Guid Id { get; }

    decimal? NumberValue { get; }
    string? TextValue { get; }
    string? TextEnglish { get; }
    decimal? ProdValue { get; }
}

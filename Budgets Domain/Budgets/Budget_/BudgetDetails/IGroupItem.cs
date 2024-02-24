namespace Krypton.Budgets.Domain.Budgets.Budget_.BudgetDetails;

public interface IGroupItem
{
    Guid Id { get; }

    string? Description { get; }
    string? DescEnglish { get; }

    IEnumerable<ICategoryItem> Categories { get; }
}

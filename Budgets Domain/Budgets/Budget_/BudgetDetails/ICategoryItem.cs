namespace Krypton.Budgets.Domain.Budgets.Budget_.BudgetDetails;

public interface ICategoryItem
{
    Guid Id { get; }

    string Formula { get; }
    string? Description { get; }
    string? DescEnglish { get; }

    IEnumerable<IValueDefItem> Defs { get; }
    IEnumerable<IItemItem> Items { get; }
}

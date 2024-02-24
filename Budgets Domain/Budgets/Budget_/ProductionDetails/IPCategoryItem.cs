namespace Krypton.Budgets.Domain.Budgets.Budget_.ProductionDetails;

public interface IPCategoryItem
{
    Guid Id { get; }

    string Formula { get; }
    string? Description { get; }
    string? DescEnglish { get; }

    IEnumerable<IPValueDefItem> Defs { get; }
    IEnumerable<IPItemItem> Items { get; }
}

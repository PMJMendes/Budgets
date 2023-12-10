namespace Krypton.Budgets.Domain.Budgets.Budget_.ProductionDetails;

public interface IPGroupItem
{
    Guid Id { get; }

    string Description { get; }
    string? DescEnglish { get; }

    IEnumerable<IPCategoryItem> Categories { get; }
}

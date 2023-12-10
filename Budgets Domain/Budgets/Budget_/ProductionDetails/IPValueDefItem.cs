using ValueType = Krypton.Budgets.Shared.ValueType;

namespace Krypton.Budgets.Domain.Budgets.Budget_.ProductionDetails;

public interface IPValueDefItem
{
    Guid Id { get; }

    ValueType Type { get; }
    string Description { get; }
    string? DescEnglish { get; }
}

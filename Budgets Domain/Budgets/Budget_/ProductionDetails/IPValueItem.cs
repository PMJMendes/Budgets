namespace Krypton.Budgets.Domain.Budgets.Budget_.ProductionDetails;

public interface IPValueItem
{
    Guid Id { get; }

    decimal? NumberValue { get; }
    string? TextValue { get; }
    string? TextEnglish { get; }
}

using Krypton.Budgets.Domain.Budgets.Value_;

namespace Krypton.Budgets.Domain.Budgets.Budget_.UpdateBudget;

public interface IValueArgs
{
    Guid? Id { get; }

    decimal? NumberValue { get; }
    string? TextValue { get; }
    string? TextEnglish { get; }
    decimal? ProdValue { get; }

    internal sealed ValueData ToValueData() => new(
        Id ?? Guid.Empty,
        NumberValue,
        TextValue,
        TextEnglish,
        ProdValue
    );
}

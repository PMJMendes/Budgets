namespace Krypton.Budgets.Domain.Budgets.Value_;

internal record struct ValueData(
    Guid Id,
    decimal? NumberValue,
    string? TextValue,
    string? TextEnglish,
    decimal? ProdValue)
{
    public static readonly ValueData Empty = new();
}

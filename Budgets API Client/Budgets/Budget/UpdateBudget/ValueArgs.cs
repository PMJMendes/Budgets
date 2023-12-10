namespace Krypton.Budgets.Blazor.APIClient.Budgets.Budget.UpdateBudget;

public class ValueArgs
{
    public ValueArgs(Guid? id, decimal? numberValue, string? textValue, string? textEnglish, decimal? prodValue)
    {
        Id = id;
        NumberValue = numberValue;
        TextValue = textValue;
        TextEnglish = textEnglish;
        ProdValue = prodValue;
    }

    public Guid? Id { get; private init; }
    public decimal? NumberValue { get; private init; }
    public string? TextValue { get; private init; }
    public string? TextEnglish { get; private init; }
    public decimal? ProdValue { get; private init; }
}

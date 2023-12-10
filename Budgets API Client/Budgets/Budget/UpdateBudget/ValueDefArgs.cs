namespace Krypton.Budgets.Blazor.APIClient.Budgets.Budget.UpdateBudget;

public class ValueDefArgs
{
    public ValueDefArgs(Guid id, string type, string description, string? descEnglish, string? bCAFormula)
    {
        Id = id;
        Type = type;
        Description = description;
        DescEnglish = descEnglish;
        BCAFormula = bCAFormula;
    }

    public Guid Id { get; private init; }
    public string Type { get; private init; }
    public string Description { get; private init; }
    public string? DescEnglish { get; private init; }
    public string? BCAFormula { get; private init; }
}

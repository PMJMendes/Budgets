namespace Krypton.Budgets.Blazor.APIClient.Budgets.Budget.UpdateBudget;

public class CategoryArgs
{
    public CategoryArgs(Guid id, string formula, string description, string? descEnglish,
        IEnumerable<ValueDefArgs> valueDefArgs, IEnumerable<ItemArgs> itemArgs)
    {
        Id = id;
        Formula = formula;
        Description = description;
        DescEnglish = descEnglish;
        ValueDefArgs = valueDefArgs;
        ItemArgs = itemArgs;
    }

    public Guid Id { get; private init; }
    public string Formula { get; private init; }
    public string Description { get; private init; }
    public string? DescEnglish { get; private init; }
    public IEnumerable<ValueDefArgs> ValueDefArgs { get; private init; }
    public IEnumerable<ItemArgs> ItemArgs { get; private init; }
}

namespace Krypton.Budgets.Blazor.APIClient.Budgets.Budget.UpdateBudget;

public class GroupArgs
{
    public GroupArgs(Guid id, string description, string? descEnglish, IEnumerable<CategoryArgs> categoryArgs)
    {
        Id = id;
        Description = description;
        DescEnglish = descEnglish;
        CategoryArgs = categoryArgs;
    }

    public Guid Id { get; set; }
    public string Description { get; set; }
    public string? DescEnglish { get; set; }
    public IEnumerable<CategoryArgs> CategoryArgs { get; set; }
}

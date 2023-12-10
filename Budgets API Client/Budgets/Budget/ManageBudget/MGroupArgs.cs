namespace Krypton.Budgets.Blazor.APIClient.Budgets.Budget.ManageBudget;

public class MGroupArgs
{
    public MGroupArgs(Guid id, IEnumerable<MCategoryArgs> categoryArgs)
    {
        Id = id;
        CategoryArgs = categoryArgs;
    }

    public Guid Id { get; set; }
    public IEnumerable<MCategoryArgs> CategoryArgs { get; set; }
}

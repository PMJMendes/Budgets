namespace Krypton.Budgets.Blazor.APIClient.Budgets.Budget.ManageBudget;

public class MCategoryArgs
{
    public MCategoryArgs(Guid id, IEnumerable<MItemArgs> itemArgs)
    {
        Id = id;
        ItemArgs = itemArgs;
    }

    public Guid Id { get; private init; }
    public IEnumerable<MItemArgs> ItemArgs { get; private init; }
}

namespace Krypton.Budgets.Blazor.APIClient.Budgets.Budget.ManageBudget;

public class MItemArgs
{
    public MItemArgs(Guid id, IEnumerable<MCostArgs> costArgs, IEnumerable<MInvoiceArgs> invoiceArgs)
    {
        Id = id;
        CostArgs = costArgs;
        InvoiceArgs = invoiceArgs;
    }

    public Guid Id { get; private init; }
    public IEnumerable<MCostArgs> CostArgs { get; private init; }
    public IEnumerable<MInvoiceArgs> InvoiceArgs { get; private init; }
}

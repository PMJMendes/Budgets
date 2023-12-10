using Krypton.Budgets.Domain.Budgets.Budget_.ManageBudget;
using System.Text.Json.Serialization;

namespace Krypton.Budgets.API.Budgets.Budget.ManageBudget;

internal class MItemArgs : IMItemArgs
{
    public Guid? Id { get; set; }

    public List<MCostArgs>? CostArgs { get; set; }
    public List<MInvoiceArgs>? InvoiceArgs { get; set; }

    [JsonIgnore]
    IEnumerable<IMCostArgs>? IMItemArgs.CostArgs => CostArgs?.Cast<IMCostArgs>();
    [JsonIgnore]
    IEnumerable<IMInvoiceArgs>? IMItemArgs.InvoiceArgs => InvoiceArgs?.Cast<IMInvoiceArgs>();
}

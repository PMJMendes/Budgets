using Krypton.Budgets.Domain._Impl.Attributes;
using Krypton.Budgets.Domain.Budgets.Item_;
using Krypton.Budgets.Domain.Budgets.Value_;

namespace Krypton.Budgets.Domain.Budgets.Budget_.ManageBudget;

public interface IMItemArgs
{
    Guid? Id { get; }

    [Required]
    IEnumerable<IMCostArgs>? CostArgs { get; }

    [Required]
    IEnumerable<IMInvoiceArgs>? InvoiceArgs { get; }

    internal sealed ItemData ToItemData(Exception ex) => new(
        null, null, Array.Empty<ValueData>(),
        CostArgs?.Select(c => c.ToCostData()) ?? throw ex,
        InvoiceArgs?.Select(i => i.ToInvoiceData()) ?? throw ex
    );
}

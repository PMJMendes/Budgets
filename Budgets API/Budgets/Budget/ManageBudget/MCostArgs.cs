using Krypton.Budgets.Domain.Budgets.Budget_.ManageBudget;

namespace Krypton.Budgets.API.Budgets.Budget.ManageBudget;

internal class MCostArgs : IMCostArgs
{
    public Guid? Id { get; set; }

    public decimal? CostValue { get; set; }
    public string? Supplier { get; set; }
    public string? Text { get; set; }
}

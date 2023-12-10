using Krypton.Budgets.Domain.Budgets.Budget_.ManageBudget;
using Newtonsoft.Json;

namespace Krypton.Budgets.API.Budgets.Budget.ManageBudget;

internal class MCategoryArgs : IMCategoryArgs
{
    public Guid? Id { get; set; }

    public List<MItemArgs>? ItemArgs { get; set; }

    [JsonIgnore]
    IEnumerable<IMItemArgs>? IMCategoryArgs.ItemArgs => ItemArgs?.Cast<IMItemArgs>();
}

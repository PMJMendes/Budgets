using Krypton.Budgets.Domain.Budgets.Budget_.ManageBudget;
using System.Text.Json.Serialization;

namespace Krypton.Budgets.API.Budgets.Budget.ManageBudget;

internal class MGroupArgs : IMGroupArgs
{
    public Guid? Id { get; set; }

    public List<MCategoryArgs>? CategoryArgs { get; set; }

    [JsonIgnore]
    IEnumerable<IMCategoryArgs>? IMGroupArgs.CategoryArgs => CategoryArgs?.Cast<IMCategoryArgs>();
}

using Krypton.Budgets.Domain.Budgets.Budget_.UpdateBudget;
using System.Text.Json.Serialization;

namespace Krypton.Budgets.API.Budgets.Budget.UpdateBudget;

internal class GroupArgs : IGroupArgs
{
    public Guid? Id { get; set; }
    public string? Description { get; set; }
    public string? DescEnglish { get; set; }
    public List<CategoryArgs>? CategoryArgs { get; set; }

    [JsonIgnore]
    IEnumerable<ICategoryArgs>? IGroupArgs.CategoryArgs => CategoryArgs?.Cast<ICategoryArgs>();
}

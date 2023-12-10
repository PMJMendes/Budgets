using Krypton.Budgets.Domain.Budgets.Budget_.UpdateBudget;
using Newtonsoft.Json;

namespace Krypton.Budgets.API.Budgets.Budget.UpdateBudget;

internal class CategoryArgs : ICategoryArgs
{
    public Guid? Id { get; set; }
    public string? Formula { get; set; }
    public string? Description { get; set; }
    public string? DescEnglish { get; set; }
    public List<ValueDefArgs>? ValueDefArgs { get; set; }
    public List<ItemArgs>? ItemArgs { get; set; }

    [JsonIgnore]
    IEnumerable<IValueDefArgs>? ICategoryArgs.ValueDefArgs => ValueDefArgs?.Cast<IValueDefArgs>();
    [JsonIgnore]
    IEnumerable<IItemArgs>? ICategoryArgs.ItemArgs => ItemArgs?.Cast<IItemArgs>();
}

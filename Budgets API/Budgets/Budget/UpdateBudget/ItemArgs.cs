using Krypton.Budgets.Domain.Budgets.Budget_.UpdateBudget;
using System.Text.Json.Serialization;

namespace Krypton.Budgets.API.Budgets.Budget.UpdateBudget;

internal class ItemArgs : IItemArgs
{
    public Guid? Id { get; set; }
    public bool? ExcludeFromBase { get; set; }
    public bool? CanBePercent { get; set; }
    public string? Description { get; set; }
    public string? DescEnglish { get; set; }
    public decimal? Percent { get; set; }
    public decimal? BCAPercent { get; set; }
    public List<ValueArgs>? ValueArgs { get; set; }

    [JsonIgnore]
    IEnumerable<IValueArgs>? IItemArgs.ValueArgs => ValueArgs?.Cast<IValueArgs>();
}

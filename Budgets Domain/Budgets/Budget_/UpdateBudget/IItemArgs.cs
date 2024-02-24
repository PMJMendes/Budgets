using Krypton.Budgets.Domain._Impl.Attributes;
using Krypton.Budgets.Domain.Budgets.Invoice_;
using Krypton.Budgets.Domain.Budgets.Item_;

namespace Krypton.Budgets.Domain.Budgets.Budget_.UpdateBudget;

public interface IItemArgs
{
    Guid? Id { get; }

    [Required]
    bool? ExcludeFromBase { get; }
    [Required]
    bool? CanBePercent { get; }
    string? Description { get; }
    string? DescEnglish { get; }

    decimal? Percent { get; }
    decimal? BCAPercent { get; }

    [Required]
    IEnumerable<IValueArgs>? ValueArgs { get; }

    [Required]
    IEnumerable<ICostArgs>? CostArgs { get; }

    internal sealed ItemDefData ToItemDef(Exception ex) => new(
        Id ?? Guid.Empty,
        ExcludeFromBase ?? throw ex,
        CanBePercent ?? throw ex,
        Description?.Trim() ?? throw ex,
        DescEnglish
    );

    internal sealed ItemData ToItemData(Exception ex) => new(
        Percent,
        BCAPercent,
        ValueArgs?.Select(v => v.ToValueData()) ?? throw ex,
        CostArgs?.Select(c => c.ToCostData()) ?? throw ex,
        Array.Empty<InvoiceData>()
    );
}

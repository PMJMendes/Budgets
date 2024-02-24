using Krypton.Budgets.Domain._Impl.Attributes;
using Krypton.Budgets.Domain.Budgets.Category_;

namespace Krypton.Budgets.Domain.Budgets.Budget_.UpdateBudget;

public interface ICategoryArgs
{
    Guid? Id { get; }

    [Required]
    string? Formula { get; }
    string? Description { get; }
    string? DescEnglish { get; }

    [Required]
    IEnumerable<IValueDefArgs>? ValueDefArgs { get; }
    [Required]
    IEnumerable<IItemArgs>? ItemArgs { get; }

    internal sealed CategoryDefData ToCategoryDef(Exception ex) => new(
        Id ?? Guid.Empty,
        Formula?.Trim() ?? throw ex,
        Description?.Trim() ?? throw ex,
        DescEnglish,
        ValueDefArgs?.Select(v => v.ToValueDef(ex)) ?? throw ex,
        ItemArgs?.Select(i => i.ToItemDef(ex)) ?? throw ex
    );

    internal sealed CategoryData ToCategoryData(Exception ex) => new(
        ItemArgs?.Select(i => i.ToItemData(ex)) ?? throw ex
    );
}

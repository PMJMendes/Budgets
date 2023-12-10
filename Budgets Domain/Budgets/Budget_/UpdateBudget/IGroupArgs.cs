using Krypton.Budgets.Domain._Impl.Attributes;
using Krypton.Budgets.Domain.Budgets.Group_;

namespace Krypton.Budgets.Domain.Budgets.Budget_.UpdateBudget;

public interface IGroupArgs
{
    Guid? Id { get; }

    [Required]
    string? Description { get; }
    string? DescEnglish { get; }

    [Required]
    IEnumerable<ICategoryArgs>? CategoryArgs { get; }

    internal sealed GroupDefData ToGroupDef(Exception ex) => new(
        Id ?? Guid.Empty,
        Description?.Trim() ?? throw ex,
        DescEnglish,
        CategoryArgs?.Select(c => c.ToCategoryDef(ex)) ?? throw ex
    );

    internal sealed GroupData ToGroupData(Exception ex) => new(
        CategoryArgs?.Select(c => c.ToCategoryData(ex)) ?? throw ex
    );
}

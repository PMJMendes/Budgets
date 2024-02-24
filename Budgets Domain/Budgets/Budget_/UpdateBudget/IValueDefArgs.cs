using Krypton.Budgets.Domain._Impl.Attributes;
using Krypton.Budgets.Domain.Budgets.ValueDef_;
using ValueType = Krypton.Budgets.Shared.ValueType;

namespace Krypton.Budgets.Domain.Budgets.Budget_.UpdateBudget;

public interface IValueDefArgs
{
    Guid? Id { get; }

    [Required]
    ValueType? Type { get; }
    string? Description { get; }
    string? DescEnglish { get; }
    string? BCAFormula { get; }

    internal sealed ValueDefData ToValueDef(Exception ex) => new(
        Id ?? Guid.Empty,
        Type ?? throw ex,
        Description ?? throw ex,
        DescEnglish,
        BCAFormula
    );
}

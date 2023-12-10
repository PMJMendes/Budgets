using Krypton.Budgets.Domain._Impl.Attributes;
using Krypton.Budgets.Domain.Budgets.Category_;

namespace Krypton.Budgets.Domain.Budgets.Budget_.ManageBudget;

public interface IMCategoryArgs
{
    Guid? Id { get; }

    [Required]
    IEnumerable<IMItemArgs>? ItemArgs { get; }

    internal sealed CategoryData ToCategoryData(Exception ex) => new(
        ItemArgs?.Select(i => i.ToItemData(ex)) ?? throw ex
    );
}

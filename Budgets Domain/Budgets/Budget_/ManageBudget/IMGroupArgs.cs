using Krypton.Budgets.Domain._Impl.Attributes;
using Krypton.Budgets.Domain.Budgets.Group_;

namespace Krypton.Budgets.Domain.Budgets.Budget_.ManageBudget;

public interface IMGroupArgs
{
    Guid? Id { get; }

    [Required]
    IEnumerable<IMCategoryArgs>? CategoryArgs { get; }

    internal sealed GroupData ToGroupData(Exception ex) => new(
        CategoryArgs?.Select(c => c.ToCategoryData(ex)) ?? throw ex
    );
}

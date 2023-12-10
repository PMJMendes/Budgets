using Krypton.Budgets.Blazor.APIClient.Budgets.Budget.AssignManager;
using Krypton.Budgets.Blazor.UI._Shared;
using Krypton.Budgets.Blazor.UI.Budgets.BudgetPage.View;

namespace Krypton.Budgets.Blazor.UI.Budgets.BudgetPage.Assign;

public class AssignManagerModel
{
    private AssignManagerModel(Guid id, RefModel? manager)
    {
        Id = id;
        Manager = manager;
    }

    public Guid Id { get; private init; }
    public RefModel? Manager { get; set; }

    public AssignManagerArgs AsArgs => new(
        Id,
        Manager?.Id
    );

    public static AssignManagerModel Empty() => new(
        Guid.Empty,
        null
    );

    public static AssignManagerModel FromViewData(BudgetViewModel data) => new(
        data.Id,
        data.Manager
    );
}

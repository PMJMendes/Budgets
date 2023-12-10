using Krypton.Budgets.Blazor.UI._Shared;
using Krypton.Budgets.Blazor.UI.Budgets.BudgetPage.View;

namespace Krypton.Budgets.Blazor.UI.Budgets.BudgetPage.Create;

public class BudgetCreateModel
{
    private BudgetCreateModel(IEnumerable<RefModel> templates, BudgetCreateFormModel formData)
    {
        Templates = templates;
        FormData = formData;
    }

    public IEnumerable<RefModel> Templates { get; set; }
    public BudgetCreateFormModel FormData { get; set; }

    public BudgetCreateModel WithCode(string code) => new(
        Templates,
        FormData.WithCode(code)
    );

    public BudgetCreateModel WithTemplates(IEnumerable<RefModel> templates) => new(
        templates,
        FormData
    );

    public static BudgetCreateModel Empty() => new(
        Array.Empty<RefModel>(),
        BudgetCreateFormModel.Empty()
    );

    public static BudgetCreateModel ForClone(BudgetViewModel source) => new(
        Array.Empty<RefModel>(),
        BudgetCreateFormModel.FromSource(source)
    );
}

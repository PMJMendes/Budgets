using Krypton.Budgets.Blazor.APIClient.Budgets.Budget.BudgetDetails;
using Krypton.Budgets.Blazor.APIClient.Budgets.Budget.ManageBudget;
using Krypton.Budgets.Blazor.APIClient.Budgets.Budget.ProductionDetails;
using Krypton.Budgets.Shared;

namespace Krypton.Budgets.Blazor.UI.Budgets.BudgetPage._Core;

public class CostModel : ICost
{
    private CostModel(Guid id, decimal? costValue, string? supplier, string? text)
    {
        Id = id;

        CostValue = costValue;
        Supplier = supplier;
        Text = text;
    }

    public Guid Id { get; private init; }

    public decimal? CostValue { get; set; }
    public string? Supplier { get; set; }
    public string? Text { get; set; }

    decimal ICost.CostValue => CostValue ?? 0m;

    public MCostArgs AsManageArgs() => new(
        Id,
        CostValue ?? 0m,
        Supplier,
        Text
    );

    public CostModel Clone() => new(
        Id,
        CostValue,
        Supplier,
        Text
    );

    public static CostModel Empty() => new(
        Guid.Empty,
        0m,
        null,
        null
    );

    public static CostModel FromService(CostItem? item) => new(
        item?.Id ?? Guid.Empty,
        item?.CostValue ?? 0m,
        item?.Supplier,
        item?.Text
    );

    public static CostModel FromService(PCostItem? item) => new(
        item?.Id ?? Guid.Empty,
        item?.CostValue ?? 0m,
        item?.Supplier,
        item?.Text
    );
}

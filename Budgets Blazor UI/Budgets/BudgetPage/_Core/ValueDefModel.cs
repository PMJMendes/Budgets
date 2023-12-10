using Krypton.Budgets.Blazor.APIClient.Budgets.Budget.BudgetDetails;
using Krypton.Budgets.Blazor.APIClient.Budgets.Budget.ProductionDetails;
using Krypton.Budgets.Blazor.APIClient.Budgets.Budget.UpdateBudget;
using Krypton.Budgets.Shared;
using ValueType = Krypton.Budgets.Shared.ValueType;

namespace Krypton.Budgets.Blazor.UI.Budgets.BudgetPage._Core;

public class ValueDefModel : IValueDef
{
    public ValueDefModel(Guid id, int order, ValueType type, string description, string? descEnglish, string? bCAFormula)
    {
        Id = id;
        Order = order;

        Type = type;
        Description = description;
        DescEnglish = descEnglish;
        BCAFormula = bCAFormula;
    }

    public Guid Id { get; private init; }
    public int Order { get; private set; }

    public ValueType Type { get; set; }
    public string Description { get; set; }
    public string? DescEnglish { get; set; }
    public string? BCAFormula { get; set; }

    public bool IsValid => Type != ValueType._UNKNOWN &&
        !string.IsNullOrWhiteSpace(Description);

    public ValueDefArgs AsArgs() => new(
        Id,
        Type.ToString(),
        Description,
        DescEnglish,
        BCAFormula
    );

    public ValueDefModel WithOrder(int order) => new(
        Id,
        order,
        Type,
        Description,
        DescEnglish,
        BCAFormula
    );

    public ValueDefModel Clone() => new(
        Id,
        Order,
        Type,
        Description,
        DescEnglish,
        BCAFormula
    );

    public static ValueDefModel Empty() => new(
        Guid.Empty,
        0,
        ValueType._UNKNOWN,
        string.Empty,
        null,
        null
    );

    public static ValueDefModel FromService(ValueDefItem? item) => new(
        item?.Id ?? Guid.Empty,
        0,
        Enum.TryParse(item?.Type, true, out ValueType type) ? type : ValueType._UNKNOWN,
        item?.Description ?? string.Empty,
        item?.DescEnglish,
        item?.BCAFormula
    );

    public static ValueDefModel FromService(PValueDefItem? item) => new(
        item?.Id ?? Guid.Empty,
        0,
        Enum.TryParse(item?.Type, true, out ValueType type) ? type : ValueType._UNKNOWN,
        item?.Description ?? string.Empty,
        item?.DescEnglish,
        null
    );
}

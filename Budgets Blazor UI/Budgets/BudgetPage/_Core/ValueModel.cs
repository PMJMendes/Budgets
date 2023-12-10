using Krypton.Budgets.Blazor.APIClient.Budgets.Budget.BudgetDetails;
using Krypton.Budgets.Blazor.APIClient.Budgets.Budget.ProductionDetails;
using Krypton.Budgets.Blazor.APIClient.Budgets.Budget.UpdateBudget;
using Krypton.Budgets.Shared;

namespace Krypton.Budgets.Blazor.UI.Budgets.BudgetPage._Core;

public class ValueModel : IValue
{
    private readonly ItemModel _owner;

    private ValueModel(Guid id, ValueDefModel def, ItemModel owner, decimal? numberValue, string? textValue, string? textEnglish, decimal? prodValue)
    {
        Id = id;
        Def = def;
        _owner = owner;

        NumberValue = numberValue;
        TextValue = textValue;
        TextEnglish = textEnglish;
        ProdValue = prodValue;
    }

    public Guid Id { get; private init; }
    public ValueDefModel Def { get; private set; }

    public decimal? NumberValue { get; set; }
    public string? TextValue { get; set; }
    public string? TextEnglish { get; set; }
    public decimal? ProdValue { get; set; }

    public decimal BCAAdjustedValue { get; private set; }
    public bool HasErrors { get; private set; }

    IValueDef IValue.Def => Def;
    IItem IValue.Owner => _owner;
    decimal IValue.BCAAdjustedValue { set => BCAAdjustedValue = value; }
    bool IValue.HasErrors { get => HasErrors; set => HasErrors = value; }

    public ValueArgs AsArgs() => new(
        Id,
        NumberValue,
        TextValue,
        TextEnglish,
        ProdValue
    );

    public ValueModel WithDef(ValueDefModel def) => new(
        Id,
        def,
        _owner,
        NumberValue,
        TextValue,
        TextEnglish,
        ProdValue
    );

    public ValueModel WithOwner(ItemModel owner) => new(
        Id,
        Def,
        owner,
        NumberValue,
        TextValue,
        TextEnglish,
        ProdValue
    );

    public ValueModel Clone() => new(
        Id,
        Def,
        _owner,
        NumberValue,
        TextValue,
        TextEnglish,
        ProdValue
    );

    public static ValueModel Empty() => new(
        Guid.Empty,
        ValueDefModel.Empty(),
        ItemModel.Empty(),
        null,
        null,
        null,
        null
    );

    public static ValueModel FromService(ValueItem? item) => new(
        item?.Id ?? Guid.Empty,
        ValueDefModel.Empty(),
        ItemModel.Empty(),
        item?.NumberValue,
        item?.TextValue,
        item?.TextEnglish,
        item?.ProdValue
    );

    public static ValueModel FromService(PValueItem? item) => new(
        item?.Id ?? Guid.Empty,
        ValueDefModel.Empty(),
        ItemModel.Empty(),
        item?.NumberValue,
        item?.TextValue,
        item?.TextEnglish,
        item?.NumberValue
    );
}

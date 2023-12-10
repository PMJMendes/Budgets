namespace Krypton.Budgets.Shared;

public interface IValueDef
{
    int Order { get; }

    ValueType Type { get; }

    string? BCAFormula { get; }

    bool IsBCA => Type == ValueType.NUMBER &&
            BCAFormula is string formula && !string.IsNullOrWhiteSpace(formula);
}

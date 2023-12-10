using NCalc;

namespace Krypton.Budgets.Shared;

public interface IValue
{
    decimal? NumberValue { get; }

    decimal? ProdValue { get; }

    IValueDef Def { get; }

    protected IItem Owner { get; }

    protected decimal BCAAdjustedValue { set; }

    bool HasErrors { get; protected set; }

    decimal BaseValue => Def.Type switch
    {
        ValueType.NUMBER => NumberValue ?? 0m,
        _ => 0m
    };

    void RecalculateBCA(decimal bcaTotal, decimal percent)
    {
        HasErrors = false;

        var bca = 0m;

        if (Def.IsBCA)
        {
            var expr = new Expression(Def.BCAFormula)
            {
                Parameters = Owner.BaseParamValues
            };

            try
            {
                bca = bcaTotal * percent / (100m * (expr.Evaluate() as decimal? ?? BaseValue));
            }
            catch
            {
                HasErrors = true;
            }
        }

        BCAAdjustedValue = BaseValue + bca;
    }
}

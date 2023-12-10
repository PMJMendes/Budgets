using NCalc;

namespace Krypton.Budgets.Shared;

public interface IItem
{
    bool ExcludeFromBase { get; }

    bool CanBePercent { get; }

    decimal? Percent { get; }

    decimal? BCAPercent { get; }

    protected ICategory Owner { get; }

#pragma warning disable IDE1006 // Naming Styles
    protected bool _HasErrors { get; set; }
#pragma warning restore IDE1006 // Naming Styles

    protected IEnumerable<IValue> Values { get; }
    protected IEnumerable<ICost> Costs { get; }
    protected IEnumerable<IInvoice> Invoices { get; }

    decimal Value { get; protected set; }

    decimal BCAValue { get; protected set; }

    decimal ClientValue { get; protected set; }

    decimal ProdValue { get; protected set; }

    Dictionary<string, object> BaseParamValues =>
        Values.ToDictionary(v => "P" + v.Def.Order, v => v.BaseValue as object);

    Dictionary<string, object> ProdParamValues =>
        Values.ToDictionary(v => "P" + v.Def.Order, v => v.ProdValue ?? 0m as object);

    bool IsPercent => CanBePercent && Percent is decimal percent && percent != 0;

    decimal BaseValue => ExcludeFromBase || IsPercent ? 0m : Value;

    decimal UsedBCA => Value != 0 ? BCAPercent ?? 0m : 0m;

    decimal ProdDelta => BaseValue - ProdValue;

    decimal Provisional => Value - ProdValue;

    decimal CostValue => Costs.Sum(c => c.CostValue);

    decimal Available => ProdValue - CostValue;

    decimal Margin => Value - CostValue;

    decimal InvoicedValue => Invoices.Sum(i => i.InvoicedValue);

    decimal InvoiceDelta => CostValue - InvoicedValue;

    decimal ActualMargin => Value - InvoicedValue;

    bool HasErrors => _HasErrors || Values.Any(v => v.HasErrors);

    void Recalculate(decimal baseTotal)
    {
        if (!IsPercent)
        {
            return;
        }

        Value = (Percent ?? 0m) * baseTotal / 100m;
    }

    void RecalculateBase()
    {
        _HasErrors = false;

        if (IsPercent)
        {
            return;
        }

        var expr = new Expression(Owner.Formula)
        {
            Parameters = BaseParamValues
        };

        try
        {
            Value = expr.Evaluate() as decimal? ?? 0m;
        }
        catch
        {
            _HasErrors = true;
            Value = 0m;
        }
    }

    void RecalculateClientTotal(decimal bcaTotal)
    {
        ClientValue = Value;

        if (Value != 0)
        {
            BCAValue = bcaTotal * UsedBCA / 100m;
            ClientValue += BCAValue;

            foreach (var bcaVal in Values)
            {
                bcaVal.RecalculateBCA(bcaTotal, UsedBCA);
            }
        }
    }

    void RecalculateProd()
    {
        if (ExcludeFromBase || IsPercent)
        {
            ProdValue = 0m;
            return;
        }

        var expr = new Expression(Owner.Formula)
        {
            Parameters = ProdParamValues
        };

        try
        {
            ProdValue = expr.Evaluate() as decimal? ?? 0m;
        }
        catch
        {
            ProdValue = 0m;
        }
    }
}

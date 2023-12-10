namespace Krypton.Budgets.Shared;

public interface IGroup
{
    IEnumerable<ICategory> Categories { get; }

    decimal Value => Categories.Select(c => c.Value).Sum();

    decimal BaseValue => Categories.Select(c => c.BaseValue).Sum();

    decimal UsedBCA => Categories.Select(c => c.UsedBCA).Sum();

    decimal BCAValue => Categories.Select(c => c.BCAValue).Sum();

    decimal ClientValue => Categories.Select(c => c.ClientValue).Sum();

    decimal ProdValue => Categories.Select(c => c.ProdValue).Sum();

    decimal ProdDelta => BaseValue - ProdValue;

    decimal Provisional => Value - ProdValue;

    decimal CostValue => Categories.Sum(c => c.CostValue);

    decimal Available => ProdValue - CostValue;

    decimal Margin => Value - CostValue;

    decimal InvoicedValue => Categories.Sum(c => c.InvoicedValue);

    decimal InvoiceDelta => CostValue - InvoicedValue;

    decimal ActualMargin => Value - InvoicedValue;

    void Recalculate(decimal baseTotal)
    {
        foreach (var c in Categories)
        {
            c.Recalculate(baseTotal);
        }
    }

    void RecalculateBase()
    {
        foreach (var c in Categories)
        {
            c.RecalculateBase();
        }
    }

    void RecalculateClientTotal(decimal bcaTotal)
    {
        foreach (var c in Categories)
        {
            c.RecalculateClientTotal(bcaTotal);
        }
    }

    void RecalculateProd()
    {
        foreach (var c in Categories)
        {
            c.RecalculateProd();
        }
    }
}

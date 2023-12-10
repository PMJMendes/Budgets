namespace Krypton.Budgets.Shared;

public interface ICategory
{
    string Formula { get; }

    protected IEnumerable<IItem> Items { get; }

    decimal Value => Items.Sum(i => i.Value);

    decimal BaseValue => Items.Sum(i => i.BaseValue);

    decimal UsedBCA => Items.Sum(i => i.UsedBCA);

    decimal BCAValue => Items.Sum(i => i.BCAValue);

    decimal ClientValue => Items.Sum(i => i.ClientValue);

    decimal ProdValue => Items.Sum(i => i.ProdValue);

    decimal ProdDelta => BaseValue - ProdValue;

    decimal Provisional => Value - ProdValue;

    decimal CostValue => Items.Sum(c => c.CostValue);

    decimal Available => ProdValue - CostValue;

    decimal Margin => Value - CostValue;

    decimal InvoicedValue => Items.Sum(i => i.InvoicedValue);

    decimal InvoiceDelta => CostValue - InvoicedValue;

    decimal ActualMargin => Value - InvoicedValue;

    void Recalculate(decimal baseTotal)
    {
        foreach (var i in Items)
        {
            i.Recalculate(baseTotal);
        }
    }

    void RecalculateBase()
    {
        foreach (var i in Items)
        {
            i.RecalculateBase();
        }
    }

    void RecalculateClientTotal(decimal bcaTotal)
    {
        foreach (var i in Items)
        {
            i.RecalculateClientTotal(bcaTotal);
        }
    }

    void RecalculateProd()
    {
        foreach (var i in Items)
        {
            i.RecalculateProd();
        }
    }
}

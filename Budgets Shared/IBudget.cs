namespace Krypton.Budgets.Shared;

public interface IBudget
{
    string? StudioDays { get; }
    string? LocationDays { get; }
    string? OutsideDays { get; }
    string? WeekendHolidays { get; }

    decimal? ProducerPercent { get; }
    decimal? BCAPercent { get; }
    decimal? WeatherTotal { get; }
    int? NWeatherDays { get; }

    protected IEnumerable<IGroup> Groups { get; }

    decimal ProducerValue => BaseValue * (ProducerPercent ?? 0m) / 100m;

    decimal GroupsValue => Groups.Select(g => g.Value).Sum();

    decimal Value => GroupsValue + ProducerValue;

    decimal BaseValue => Groups.Select(g => g.BaseValue).Sum();

    decimal BCAValue => Value * ((BCAPercent / (100m - BCAPercent)) ?? 0m);

    decimal TargetValue => Value + BCAValue;

    decimal UsedBCA => Groups.Select(g => g.UsedBCA).Sum();

    decimal BCAProducerValue => ProducerValue * (1 + ((BCAPercent / (100m - BCAPercent)) ?? 0m));

    decimal SubTotal => Groups.Select(g => g.ClientValue).Sum();

    decimal ClientValue => SubTotal + BCAProducerValue;

    decimal? WeatherBCA => WeatherTotal * (1 + ((BCAPercent / (100m - BCAPercent)) ?? 0m));

    decimal ProdValue => Groups.Select(g => g.ProdValue).Sum();

    decimal ProdDelta => BaseValue - ProdValue;

    decimal Provisional => Value - ProdValue;

    decimal CostValue => Groups.Sum(c => c.CostValue);

    decimal Available => ProdValue - CostValue;

    decimal Margin => Value - CostValue;

    decimal MarginPercent => Value != 0m ? Margin * 100m / Value : 0m;

    decimal BaseMargin => GroupsValue - InvoicedValue;

    decimal BaseMarginPercent => GroupsValue != 0m ? BaseMargin * 100m / GroupsValue : 0m;

    decimal InvoicedValue => Groups.Sum(g => g.InvoicedValue);

    decimal InvoiceDelta => CostValue - InvoicedValue;

    decimal ActualMargin => Value - InvoicedValue;

    decimal ActualMarginPercent => CostValue != 0m ? ActualMargin * 100m / CostValue : 0m;

    decimal WeatherValue => (WeatherTotal ?? 0m) * (NWeatherDays ?? 0);

    decimal WeatherInvoiced => (WeatherBCA ?? 0m) * (NWeatherDays ?? 0);

    decimal TotalWithWeather => Value + WeatherValue;

    decimal TotalInvoiced => Value + WeatherInvoiced;

    decimal TotalMargin => ActualMargin + WeatherValue;

    decimal TotalMarginPercent => TotalWithWeather != 0m ? TotalMargin * 100m / TotalWithWeather : 0m;

    int TotalDays =>
        (int.TryParse(StudioDays, out int sd) ? sd : 0) +
        (int.TryParse(LocationDays, out int ld) ? ld : 0) +
        (int.TryParse(OutsideDays, out int od) ? od : 0) +
        (int.TryParse(WeekendHolidays, out int wd) ? wd : 0);

    void Recalculate()
    {
        RecalculateBase();

        var val = BaseValue;

        foreach (var g in Groups)
        {
            g.Recalculate(val);
        }
    }

    void RecalculateBase()
    {
        foreach (var g in Groups)
        {
            g.RecalculateBase();
        }
    }

    void RecalculateClientTotal()
    {
        Recalculate();

        var val = GroupsValue * ((BCAPercent / (100m - BCAPercent)) ?? 0m);

        foreach (var g in Groups)
        {
            g.RecalculateClientTotal(val);
        }
    }

    void RecalculateProd()
    {
        foreach (var g in Groups)
        {
            g.RecalculateProd();
        }
    }
}

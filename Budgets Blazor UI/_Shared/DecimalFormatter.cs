namespace Krypton.Budgets.Blazor.UI._Shared;

public static class DecimalFormatter
{
    public static string AsAnyValue(this decimal value) => value.ToString("N2");
    public static string AsIntValue(this decimal value) => value.ToString("D");
}

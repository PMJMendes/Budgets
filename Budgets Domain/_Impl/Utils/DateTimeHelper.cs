namespace Krypton.Budgets.Domain._Impl.Utils;

internal static class DateTimeHelper
{
    public static DateTime AtStartOfDay(this DateOnly date)
    {
        return date.ToDateTime(TimeOnly.MinValue);
    }
}

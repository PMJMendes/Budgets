namespace Krypton.Budgets.Domain._Impl.Utils;

internal static class EnumerableHelper
{
    public static IEnumerable<T> AsSingleEnumerator<T>(this T element)
    {
        yield return element;
    }
}

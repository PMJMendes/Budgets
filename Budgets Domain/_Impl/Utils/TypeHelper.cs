using System.Linq.Expressions;
using System.Reflection;

namespace Krypton.Budgets.Domain._Impl.Utils;

internal static class TypeHelper
{
    public static PropertyInfo GetProperty<T, U>(Expression<Func<T, U>> expr)
    {
        return (expr.Body as MemberExpression)?.Member as PropertyInfo ??
            throw new Exception("Property not found");
    }
}

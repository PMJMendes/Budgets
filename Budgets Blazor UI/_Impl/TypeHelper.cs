using System.Linq.Expressions;
using System.Reflection;

namespace Krypton.Budgets.Blazor.UI._Impl;

internal static class TypeHelper
{
    public static PropertyInfo? GetProperty<T, U>(Expression<Func<T, U>> expr) =>
        (expr.Body as MemberExpression)?.Member as PropertyInfo;
}

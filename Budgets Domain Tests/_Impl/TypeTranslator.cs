using System.Reflection;

namespace Krypton.Budgets.Tests.Domain._Impl;

internal class TypeTranslator
{
    private static readonly MethodInfo _castMethod = typeof(Enumerable).GetMethod(nameof(Enumerable.Cast), BindingFlags.Public | BindingFlags.Static)!;
    private static readonly MethodInfo _toHashSetMethod = typeof(Enumerable).GetMethods(BindingFlags.Public | BindingFlags.Static).
        Where(m => m.Name == nameof(Enumerable.ToHashSet) && m.GetParameters().Length == 1).Single();

    public TypeTranslator(Type type)
    {
        CastMethod = _castMethod.MakeGenericMethod(type);
        ToHashSetMethod = _toHashSetMethod.MakeGenericMethod(type);
    }

    public MethodInfo CastMethod { get; internal init; }

    public MethodInfo ToHashSetMethod { get; internal init; }
}

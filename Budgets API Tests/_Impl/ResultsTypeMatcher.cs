using ImpromptuInterface;
using Moq;

namespace Krypton.Budgets.Tests.API._Impl;

[TypeMatcher]
internal class ResultsTypeMatcher<T> : ITypeMatcher where T : class
{
    public static implicit operator T(ResultsTypeMatcher<T> _) =>
        new object().ActLike<T>();

    bool ITypeMatcher.Matches(Type type)
    {
        return typeof(T).IsAssignableFrom(type);
    }
}

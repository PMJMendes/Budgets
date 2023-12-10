using Krypton.Budgets.Domain._Base.Exceptions;
using System.Reflection;

namespace Krypton.Budgets.Domain._Impl.Exceptions;

public class RequiredException : DomainException
{
    private enum ExceptionErrors
    {
        INVALID_BLANK_VALUE
    }

    public RequiredException(PropertyInfo propRef) :
        this(propRef, ExceptionErrors.INVALID_BLANK_VALUE)
    { }

    public RequiredException(PropertyInfo propRef, Enum tag) :
        base(propRef, tag, $"Required Field Violation: {propRef.ReflectedType?.Name}.{propRef.Name}")
    { }
}

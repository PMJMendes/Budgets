using Krypton.Budgets.Domain._Base.Exceptions;
using System.Reflection;

namespace Krypton.Budgets.Domain._Impl.Exceptions;

public class TargetNotFoundException : DomainException
{
    private enum ExceptionErrors
    {
        INVALID_TARGET_ID
    }

    public TargetNotFoundException(PropertyInfo propRef, Type type) :
        this(propRef, ExceptionErrors.INVALID_TARGET_ID, type)
    { }

    public TargetNotFoundException(PropertyInfo propRef, Enum tag, Type type) :
        base(propRef, tag, $"Target Not Found: {type.Name}")
    { }
}

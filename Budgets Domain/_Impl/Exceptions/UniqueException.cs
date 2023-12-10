using Krypton.Budgets.Domain._Base.Exceptions;
using System.Reflection;

namespace Krypton.Budgets.Domain._Impl.Exceptions;

public class UniqueException : DomainException
{
    private enum ExceptionErrors
    {
        VALUE_NOT_UNIQUE,
        VALUE_SET_NOT_UNIQUE
    }

    public UniqueException(PropertyInfo propRef) : this(propRef, ExceptionErrors.VALUE_NOT_UNIQUE) { }

    public UniqueException(PropertyInfo propRef, Enum tag) :
        base(propRef, tag, $"Unique Field Violation: {propRef.ReflectedType?.Name}.{propRef.Name}")
    { }

    public UniqueException(IEnumerable<PropertyInfo> errors) :
        base(errors.Select<PropertyInfo, (Enum, PropertyInfo)>(e => new(ExceptionErrors.VALUE_SET_NOT_UNIQUE, e)),
            $"Uniqueness Violation: multiple fields")
    { }
}

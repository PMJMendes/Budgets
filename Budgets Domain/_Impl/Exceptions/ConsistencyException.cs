using Krypton.Budgets.Domain._Base.Exceptions;
using System.Reflection;

namespace Krypton.Budgets.Domain._Impl.Exceptions;

public class ConsistencyException : DomainException
{
    public ConsistencyException(PropertyInfo propRef, Enum tag, string message) :
        base(propRef, tag, $"Inconsistent Entity Behavior - {propRef.ReflectedType?.Name}: {message}")
    { }
}

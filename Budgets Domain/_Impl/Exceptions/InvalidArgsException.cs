using Krypton.Budgets.Domain._Base.Exceptions;
using System.Reflection;

namespace Krypton.Budgets.Domain._Impl.Exceptions;

public class InvalidArgsException : DomainException
{
    public InvalidArgsException(PropertyInfo propRef, Enum tag, string msg) : base(propRef, tag, msg) { }

    public InvalidArgsException(IEnumerable<(Enum Tag, PropertyInfo PropRef)> errors, string msg) : base(errors, msg) { }
}

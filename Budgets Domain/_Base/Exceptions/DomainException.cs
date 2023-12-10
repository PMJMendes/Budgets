using System.Reflection;

namespace Krypton.Budgets.Domain._Base.Exceptions;

public abstract class DomainException : Exception
{
    public IEnumerable<(Enum Tag, PropertyInfo PropRef)> Errors { get; private init; }

    public DomainException(PropertyInfo propRef, Enum tag, string msg) : base(msg)
    {
        Errors = new[] { (tag, propRef) };
    }

    public DomainException(IEnumerable<(Enum Tag, PropertyInfo PropRef)> errors, string msg) : base(msg)
    {
        Errors = errors;
    }
}

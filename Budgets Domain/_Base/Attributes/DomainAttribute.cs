using System.Reflection;

namespace Krypton.Budgets.Domain._Base.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public abstract class DomainAttribute : Attribute
{
    public abstract IEnumerable<(PropertyInfo Property, IEnumerable<Enum> Errors)> IsValid(PropertyInfo property,
        object? value);
}

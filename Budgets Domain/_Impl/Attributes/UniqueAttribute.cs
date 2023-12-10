using Krypton.Budgets.Domain._Base.Attributes;
using System.Reflection;

namespace Krypton.Budgets.Domain._Impl.Attributes;

public class UniqueAttribute : DomainAttribute
{
    public string? Combinations { get; set; }

    public override IEnumerable<(PropertyInfo Property, IEnumerable<Enum> Errors)> IsValid(PropertyInfo property,
        object? value)
    {
        // Uniqueness validation happens in the persistence layer
        return Enumerable.Empty<(PropertyInfo Property, IEnumerable<Enum> Errors)>();
    }
}

using Krypton.Budgets.Domain._Base.Attributes;
using System.Reflection;

namespace Krypton.Budgets.Domain._Impl.Attributes;

public class ExternalAttribute : DomainAttribute
{

    public override IEnumerable<(PropertyInfo Property, IEnumerable<Enum> Errors)> IsValid(PropertyInfo property,
        object? value)
    {
        // External attributes don't need any additional validation
        return Enumerable.Empty<(PropertyInfo Property, IEnumerable<Enum> Errors)>();
    }
}

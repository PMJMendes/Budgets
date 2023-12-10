using Krypton.Budgets.Domain._Base.Attributes;
using Krypton.Budgets.Domain._Impl.Utils;
using System.Reflection;

namespace Krypton.Budgets.Domain._Impl.Attributes;

public class RequiredAttribute : DomainAttribute
{
    private enum ValidationErrors
    {
        VALUE_REQUIRED
    }

    public override IEnumerable<(PropertyInfo Property, IEnumerable<Enum> Errors)> IsValid(PropertyInfo property,
        object? value)
    {
        if (value == null)
        {
            yield return new(property, ((Enum)ValidationErrors.VALUE_REQUIRED).AsSingleEnumerator());
            yield break;
        }

        if (value is string str && string.IsNullOrWhiteSpace(str))
        {
            yield return new(property, ((Enum)ValidationErrors.VALUE_REQUIRED).AsSingleEnumerator());
        }

        if (value is Guid id && id == Guid.Empty)
        {
            yield return new(property, ((Enum)ValidationErrors.VALUE_REQUIRED).AsSingleEnumerator());
        }
    }
}

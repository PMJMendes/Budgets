using Krypton.Budgets.Domain._Base.Attributes;
using System.Reflection;

namespace Krypton.Budgets.Domain._Impl.Attributes;

public class ValidateAttribute : DomainAttribute
{
    public override IEnumerable<(PropertyInfo Property, IEnumerable<Enum> Errors)> IsValid(PropertyInfo property,
        object? value)
    {
        return property.PropertyType.GetProperties().
            SelectMany(prop => ValidateProp(prop, value));
    }

    private static IEnumerable<(PropertyInfo Property, IEnumerable<Enum> Errors)> ValidateProp(PropertyInfo prop, object? value)
    {
        return prop.GetCustomAttributes<DomainAttribute>().
                SelectMany(attr => attr.IsValid(prop, prop.GetValue(value)));
    }
}

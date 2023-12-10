using NHibernate.Mapping.ByCode;
using System.Reflection;

namespace Krypton.Budgets.Persistence.Implementation;

internal static class ReflectionExtensions
{
    public static bool IsEnum(this PropertyInfo thisProp)
    {
        return thisProp.PropertyType.IsEnum ||
                (thisProp.PropertyType.IsGenericType &&
                thisProp.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) &&
                thisProp.PropertyType.GetGenericArguments()[0].IsEnum);
    }

    public static PropertyInfo? GetInverseReferenceProperty(this MemberInfo thisMember)
    {
        var type = thisMember.GetPropertyOrFieldType();
        if (type.DetermineCollectionElementOrDictionaryValueType() is null)
            return null;

        foreach (PropertyInfo p in type.GetGenericArguments()[0].GetProperties())
            if (p.PropertyType == thisMember.DeclaringType)
                return p;

        return null;
    }

    public static PropertyInfo? GetInverseCollectionProperty(this MemberInfo thisMember)
    {
        Type lrefType;
        Type lrefInversePropType;

        lrefType = thisMember.GetPropertyOrFieldType();
        if (lrefType.DetermineCollectionElementOrDictionaryValueType() is null)
            return null;

        lrefInversePropType = lrefType.GetGenericTypeDefinition().MakeGenericType(thisMember.DeclaringType!);

        foreach (PropertyInfo p in lrefType.GetGenericArguments()[0].GetProperties())
            if (p.PropertyType == lrefInversePropType)
                return p;

        return null;
    }
}

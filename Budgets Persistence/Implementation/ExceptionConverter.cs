using NHibernate.Exceptions;

namespace Krypton.Budgets.Persistence.Implementation;

internal class ExceptionConverter : ISQLExceptionConverter
{
    public Exception Convert(AdoExceptionContextInfo adoExceptionContextInfo)
    {
        var ex = adoExceptionContextInfo.SqlException;

        //var propRef = GetPropRefFromException(ex);
        //if (propRef != null)
        //{
        //    return ex.SqlState switch
        //    {
        //        PostgresErrorCodes.UniqueViolation => new UniqueException(propRef),
        //        PostgresErrorCodes.CheckViolation or PostgresErrorCodes.NotNullViolation => new RequiredException(propRef),
        //        _ => ex,
        //    };
        //}

        //var propRefs = GetMultiplePropRefsFromException(ex);
        //if (propRefs != null)
        //{
        //    return ex.SqlState switch
        //    {
        //        PostgresErrorCodes.UniqueViolation => new UniqueException(propRefs),
        //        _ => ex,
        //    };
        //}

        return ex;
    }

    //private static PropertyInfo? GetPropRefFromException(SqlException ex)
    //{
    //    var maybeMapping = NHibernateBridge.GLOBAL_CONFIG?.ClassMappings.
    //        Where(cm => cm.Table.Name == ex.TableName).
    //        FirstOrDefault();

    //    if (maybeMapping is PersistentClass mapping)
    //    {
    //        var maybeProp =
    //            mapping.PropertyIterator.Where(p => p.ColumnIterator.Any(c => ex.ColumnName == c.Text)).FirstOrDefault() ??
    //            mapping.PropertyIterator.Where(p => p.ColumnIterator.Any(c => ex.ConstraintName == mapping.Table.Name + '_' + c.Text + "_key")).FirstOrDefault() ??
    //            mapping.PropertyIterator.Where(p => p.ColumnIterator.Any(c => ex.ConstraintName == mapping.Table.Name + '_' + c.Text + "_check")).FirstOrDefault();

    //        if (maybeProp is Property prop)
    //        {
    //            return mapping.MappedClass.GetProperties().Where(p => p.Name == prop.Name).FirstOrDefault();
    //        }

    //        return null;
    //    }

    //    return null;
    //}

    //private static IEnumerable<PropertyInfo> GetMultiplePropRefsFromException(PostgresException ex)
    //{
    //    var maybeMapping = NHibernateBridge.GLOBAL_CONFIG?.ClassMappings.
    //        Where(cm => cm.Table.Name == ex.TableName).
    //        FirstOrDefault();

    //    if (maybeMapping is PersistentClass mapping)
    //    {
    //        var parts = ex.ConstraintName?.Split('_').
    //            Where(part => part != mapping.Table.Name && part != "key") ?? Array.Empty<string>();

    //        var partCombos = Enumerable.Range(0, 1 << parts.Count())
    //            .Select(index => string.Join('_', parts.Where((v, i) => (index & (1 << i)) != 0))).
    //            Where(combo => ex.ConstraintName?.Contains(combo) ?? false);

    //        var props = partCombos.Select(part => mapping.PropertyIterator.
    //            Where(p => p.ColumnIterator.Any(c => part == c.Text)).
    //            FirstOrDefault()).Where(p => p != null);

    //        return mapping.MappedClass.GetProperties().Where(p => props.Any(prop => p.Name == prop?.Name));
    //    }

    //    return Enumerable.Empty<PropertyInfo>();
    //}
}

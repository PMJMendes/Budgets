using NHibernate;
using NHibernate.Engine;
using NHibernate.SqlTypes;
using NHibernate.Type;
using NHibernate.UserTypes;
using System.Data;
using System.Data.Common;

namespace Krypton.Budgets.Persistence.Implementation;

internal class DateOnlyMapper : CustomType
{
    public DateOnlyMapper() : base(typeof(Impl), null) { }

    private class Impl : IUserType
    {
        SqlType[] IUserType.SqlTypes => new[] { SqlTypeFactory.Date };

        Type IUserType.ReturnedType => typeof(DateOnly);

        bool IUserType.Equals(object x, object y) => ((DateOnly?)x)?.Equals((DateOnly)y) ?? y is null;

        int IUserType.GetHashCode(object x) => x?.GetHashCode() ?? default;

        object IUserType.NullSafeGet(DbDataReader rs, string[] names, ISessionImplementor session, object owner)
        {
            var obj = NHibernateUtil.DateTime.NullSafeGet(rs, names, session, owner);

            return obj is null ? null! : DateOnly.FromDateTime((DateTime)obj);
        }

        void IUserType.NullSafeSet(DbCommand cmd, object value, int index, ISessionImplementor session)
        {
            if (value is null)
            {
                ((IDataParameter)cmd.Parameters[index]).Value = DBNull.Value;
            }
            else
            {
                ((IDataParameter)cmd.Parameters[index]).Value = ((DateOnly)value).ToDateTime(TimeOnly.MinValue);
            }
        }

        object IUserType.DeepCopy(object value) => DateOnly.FromDayNumber(((DateOnly)value).DayNumber);

        bool IUserType.IsMutable => false;

        object IUserType.Replace(object original, object target, object owner) => original;

        object IUserType.Assemble(object cached, object owner) => cached;

        object IUserType.Disassemble(object value) => value;
    }
}

using Krypton.Budgets.Domain._Ports.Persistence;
using Krypton.Budgets.Domain._Ports.Security;
using Microsoft.Extensions.Logging;
using NHibernate;
using NHibernate.SqlCommand;
using NHibernate.Type;
using NHibernate.Util;
using System.Reflection;

namespace Krypton.Budgets.Persistence.Implementation;

internal class EntityManagementInterceptor : EmptyInterceptor
{
    private readonly IServiceProvider sp;
    private readonly ILogger<EntityManagementInterceptor>? logger;
    private readonly IDomainExplorer domain;
    private readonly ISecurity security;

    private ISession? session;

    public EntityManagementInterceptor(IServiceProvider sp, IDomainExplorer domain, ISecurity security, ILogger<EntityManagementInterceptor>? logger = null)
    {
        this.sp = sp;
        this.logger = logger;
        this.domain = domain;
        this.security = security;
    }

    public override void SetSession(ISession session)
    {
        this.session = session;
    }

    public override bool OnFlushDirty(object entity, object id, object[] currentState, object[] previousState, string[] propertyNames, IType[] types)
    {
        foreach (int i in propertyNames.
            Select((prop, index) => (Name: prop, Index: index)).
            Where(val => val.Name == domain.WhenUpdated.Name).
            Select(val => val.Index))
        {
            currentState[i] = DateTime.Now;
        }

        foreach (int i in propertyNames.
            Select((prop, index) => (Name: prop, Index: index)).
            Where(val => val.Name == domain.WhoUpdated.Name).
            Select(val => val.Index))
        {
            currentState[i] = security.LoginTag;
        }

        return true;
    }

    public override bool OnSave(object entity, object id, object[] state, string[] propertyNames, IType[] types)
    {
        foreach (int i in propertyNames.
            Select((prop, index) => (Name: prop, Index: index)).
            Where(val => val.Name == domain.WhenCreated.Name || val.Name == domain.WhenUpdated.Name).
            Select(val => val.Index))
        {
            state[i] = DateTime.Now;
        }

        foreach (int i in propertyNames.
            Select((prop, index) => (Name: prop, Index: index)).
            Where(val => val.Name == domain.WhoCreated.Name || val.Name == domain.WhoUpdated.Name).
            Select(val => val.Index))
        {
            state[i] = security.LoginTag;
        }

        return true;
    }

    public override object Instantiate(string clazz, object id)
    {
        if (session is not null)
        {
            if (domain.DomainEntites.FirstOrDefault(x => x.FullName == clazz) is TypeInfo type)
            {
                foreach (var c in type.GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance).Where(x => x.GetParameters().Any()))
                {
                    var services = c.GetParameters().Select(p => sp.GetService(p.ParameterType));
                    if (services.All(s => s is not null))
                    {
                        var instance = c.Invoke(services.ToArray());
                        session.SessionFactory.GetClassMetadata(clazz).SetIdentifier(instance, id);
                        return instance;
                    }
                }
            }
        }

        return base.Instantiate(clazz, id);
    }

    public override SqlString OnPrepareStatement(SqlString sql)
    {
        logger?.LogTrace("SQL: {Sql}", sql);

        return sql;
    }
}

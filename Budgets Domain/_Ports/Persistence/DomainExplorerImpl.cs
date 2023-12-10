using Krypton.Budgets.Domain._Base.Entities;
using Krypton.Budgets.Domain._Base.Interfaces;
using Krypton.Budgets.Domain._Impl.Utils;
using System.Reflection;

namespace Krypton.Budgets.Domain._Ports.Persistence;

internal class DomainExplorerImpl : IDomainExplorer
{
    private static readonly int SUBDOMAIN_INDEX = 3;

    IEnumerable<TypeInfo> IDomainExplorer.DomainEntites
    {
        get
        {
            var entities = typeof(IEntity).Assembly.DefinedTypes.
                Where(t => t.IsAssignableTo(typeof(IEntity)));
            return typeof(IEntity).Assembly.DefinedTypes.
                Where(t => entities.Any(e => e.Namespace == t.Namespace));
        }
    }

    public Func<Type, string> GetEntitySubdomain => type => type.Namespace?.Split('.')[SUBDOMAIN_INDEX] ?? "";

    PropertyInfo IDomainExplorer.DomainIdentifier => TypeHelper.GetProperty((BaseEntity i) => i.Id);

    PropertyInfo IDomainExplorer.WhenCreated => TypeHelper.GetProperty((BaseEntity i) => i.WhenCreated);

    public PropertyInfo WhoCreated => TypeHelper.GetProperty((BaseEntity i) => i.WhoCreated);

    PropertyInfo IDomainExplorer.WhenUpdated => TypeHelper.GetProperty((BaseEntity i) => i.WhenUpdated);

    public PropertyInfo WhoUpdated => TypeHelper.GetProperty((BaseEntity i) => i.WhoUpdated);
}

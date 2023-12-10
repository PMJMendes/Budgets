using System.Reflection;

namespace Krypton.Budgets.Domain._Ports.Persistence;

public interface IDomainExplorer
{
    IEnumerable<TypeInfo> DomainEntites { get; }
    Func<Type, string> GetEntitySubdomain { get; }

    PropertyInfo DomainIdentifier { get; }
    PropertyInfo WhenCreated { get; }
    PropertyInfo WhoCreated { get; }
    PropertyInfo WhenUpdated { get; }
    PropertyInfo WhoUpdated { get; }
}

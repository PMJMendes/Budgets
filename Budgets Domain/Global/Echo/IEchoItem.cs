using Krypton.Budgets.Domain._Base.Interfaces;
using Krypton.Budgets.Domain._Ports.Security;

namespace Krypton.Budgets.Domain.Global.Echo;

public interface IEchoItem : IQueryResultItem
{
    public string Name { get; }
    public SecurityLevel? Level { get; }
}

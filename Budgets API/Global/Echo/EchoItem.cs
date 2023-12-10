using Krypton.Budgets.Domain._Ports.Security;
using Krypton.Budgets.Domain.Global.Echo;

namespace Krypton.Budgets.API.Global.Echo;

internal readonly struct EchoItem : IEchoItem
{
    public EchoItem(IEchoItem source)
    {
        Name = source.Name;
        Level = source.Level;
    }

    public string Name { get; private init; }
    public SecurityLevel? Level { get; private init; }
}

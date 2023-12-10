using Krypton.Budgets.Domain._Base.Interfaces;
using Krypton.Budgets.Domain._Ports.Security;

namespace Krypton.Budgets.Domain.Users.User_.ChangeSecurityLevel;

public interface IChangeSecurityLevelArgs : ITargetArgs
{
    SecurityLevel? NewLevel { get; }
}

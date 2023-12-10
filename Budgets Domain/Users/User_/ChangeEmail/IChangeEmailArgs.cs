using Krypton.Budgets.Domain._Base.Interfaces;

namespace Krypton.Budgets.Domain.Users.User_.ChangeEmail;

public interface IChangeEmailArgs : ITargetArgs
{
    string? NewEmail { get; }
}

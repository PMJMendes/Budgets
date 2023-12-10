using Krypton.Budgets.Domain._Base.Interfaces;

namespace Krypton.Budgets.Domain.Users.User_.ChangeEmail;

public interface IChangeEmailResults : IOpResults
{
    UserState State { get; }
}

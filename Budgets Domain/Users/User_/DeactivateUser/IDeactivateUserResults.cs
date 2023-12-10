using Krypton.Budgets.Domain._Base.Interfaces;

namespace Krypton.Budgets.Domain.Users.User_.DeactivateUser;

public interface IDeactivateUserResults : IOpResults
{
    UserState State { get; }
}

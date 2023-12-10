using Krypton.Budgets.Domain._Base.Interfaces;
using Krypton.Budgets.Domain.Users.User_;

namespace Krypton.Budgets.Domain.Global.CreateUser;

public interface ICreateUserResults : IOpResults
{
    Guid NewUserId { get; }

    UserState State { get; }
}

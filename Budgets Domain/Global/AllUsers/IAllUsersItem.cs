using Krypton.Budgets.Domain._Base.Interfaces;
using Krypton.Budgets.Domain._Ports.Security;
using Krypton.Budgets.Domain.Users.User_;

namespace Krypton.Budgets.Domain.Global.AllUsers;

public interface IAllUsersItem : IQueryResultItem
{
    Guid Id { get; }

    string Email { get; }

    SecurityLevel Level { get; }

    string FullName { get; }

    UserState State { get; }
}

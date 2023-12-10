using Krypton.Budgets.Domain._Base.Interfaces;
using Krypton.Budgets.Domain._Ports.Security;

namespace Krypton.Budgets.Domain.Users.User_.UserDetails;

public interface IUserDetailsItem : IQueryResultItem
{
    Guid Id { get; }
    string Email { get; }
    SecurityLevel Level { get; }
    string FullName { get; }
    UserState State { get; }
}

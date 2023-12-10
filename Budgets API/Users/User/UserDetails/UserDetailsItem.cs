using Krypton.Budgets.Domain._Ports.Security;
using Krypton.Budgets.Domain.Users.User_;
using Krypton.Budgets.Domain.Users.User_.UserDetails;

namespace Krypton.Budgets.API.Users.User.UserDetails;

internal readonly struct UserDetailsItem : IUserDetailsItem
{
    public UserDetailsItem(IUserDetailsItem source)
    {
        Id = source.Id;
        Email = source.Email;
        Level = source.Level;
        FullName = source.FullName;
        State = source.State;
    }

    public Guid Id { get; private init; }

    public string Email { get; private init; }

    public SecurityLevel Level { get; private init; }

    public string FullName { get; private init; }

    public UserState State { get; private init; }
}

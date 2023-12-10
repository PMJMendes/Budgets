using Krypton.Budgets.Domain._Ports.Security;
using Krypton.Budgets.Domain.Global.CreateUser;
using Krypton.Budgets.Domain.Users.User_;

namespace Krypton.Budgets.API.Global.CreateUser;

internal readonly struct CreateUserResults : ICreateUserResults
{
    public CreateUserResults(ICreateUserResults source, CreateUserArgs? input)
    {
        Email = input?.Email;
        Level = input?.Level;
        FullName = input?.FullName;

        NewUserId = source.NewUserId;
        State = source.State;
    }

    public string? Email { get; private init; }
    public SecurityLevel? Level { get; private init; }
    public string? FullName { get; private init; }

    public Guid NewUserId { get; private init; }
    public UserState State { get; private init; }
}

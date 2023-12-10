using Krypton.Budgets.Domain._Ports.Security;
using Krypton.Budgets.Domain.Users.User_;
using Krypton.Budgets.Domain.Users.User_.ChangeEmail;

namespace Krypton.Budgets.API.Users.User.UpdateUser;

internal readonly struct UpdateUserResults : IChangeEmailResults
{
    public UpdateUserResults(IChangeEmailResults source, UpdateUserArgs? input)
    {
        Id = input?.TargetId;
        Email = input?.NewEmail;
        Level = input?.NewLevel;
        Name = input?.NewName;

        State = source.State;
    }

    public Guid? Id { get; private init; }
    public string? Email { get; private init; }
    public SecurityLevel? Level { get; private init; }
    public string? Name { get; private init; }

    public UserState State { get; private init; }
}

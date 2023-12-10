using Krypton.Budgets.Domain.Users.User_;
using Krypton.Budgets.Domain.Users.User_.DeactivateUser;

namespace Krypton.Budgets.API.Users.User.DeactivateUser;

internal readonly struct DeactivateUserResults : IDeactivateUserResults
{
    public DeactivateUserResults(IDeactivateUserResults source)
    {
        State = source.State;
    }

    public UserState State { get; private init; }
}

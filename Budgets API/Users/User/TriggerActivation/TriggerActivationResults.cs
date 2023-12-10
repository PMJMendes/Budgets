using Krypton.Budgets.Domain.Users.User_;
using Krypton.Budgets.Domain.Users.User_.TriggerActivation;

namespace Krypton.Budgets.API.Users.User.TriggerActivation;

internal readonly struct TriggerActivationResults : ITriggerActivationResults
{
    public TriggerActivationResults(ITriggerActivationResults source)
    {
        State = source.State;
    }

    public UserState State { get; private init; }
}

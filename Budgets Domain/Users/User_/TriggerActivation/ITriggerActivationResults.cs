using Krypton.Budgets.Domain._Base.Interfaces;

namespace Krypton.Budgets.Domain.Users.User_.TriggerActivation;

public interface ITriggerActivationResults : IOpResults
{
    UserState State { get; }
}

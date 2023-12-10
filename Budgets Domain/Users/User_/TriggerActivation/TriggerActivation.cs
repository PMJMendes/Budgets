using Krypton.Budgets.Domain._Base.Interfaces;
using Krypton.Budgets.Domain._Base.Services;
using Krypton.Budgets.Domain._Impl.Interfaces;
using Microsoft.Extensions.Logging;

namespace Krypton.Budgets.Domain.Users.User_.TriggerActivation;

internal class TriggerActivation : BaseOperation<TriggerActivation, ITargetArgs, ITriggerActivationResults>, ITriggerActivation
{
    public TriggerActivation(IContext context, ILogger<TriggerActivation> logger) : base(context, logger) { }

    protected override void AssertPermissions() => AssertIsAdmin();

    protected override async Task<V> InnerExecute<V>(ITargetArgs? args, Func<ITriggerActivationResults, V> resultsFactory)
    {
        var user = await GetTarget<User>(args);

        await user.TriggerActivationAsync();

        await context.Persistence.FlushChangesAsync();

        return resultsFactory(new Result(
            user.State
        ));
    }

    private record struct Result(
        UserState State
    ) : ITriggerActivationResults;
}

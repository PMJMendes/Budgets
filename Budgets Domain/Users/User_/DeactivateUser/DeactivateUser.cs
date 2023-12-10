using Krypton.Budgets.Domain._Base.Interfaces;
using Krypton.Budgets.Domain._Base.Services;
using Krypton.Budgets.Domain._Impl.Interfaces;
using Microsoft.Extensions.Logging;

namespace Krypton.Budgets.Domain.Users.User_.DeactivateUser;

internal class DeactivateUser : BaseOperation<DeactivateUser, ITargetArgs, IDeactivateUserResults>, IDeactivateUser
{
    public DeactivateUser(IContext context, ILogger<DeactivateUser> logger) : base(context, logger) { }

    protected override void AssertPermissions() => AssertIsAdmin();

    protected override async Task<V> InnerExecute<V>(ITargetArgs? args, Func<IDeactivateUserResults, V> resultsFactory)
    {
        var user = await GetTarget<User>(args);

        await user.DeactivateAsync();

        await context.Persistence.FlushChangesAsync();

        return resultsFactory(new Result(
            user.State
        ));
    }

    private record struct Result(
        UserState State
    ) : IDeactivateUserResults;
}

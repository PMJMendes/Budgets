using Krypton.Budgets.Domain._Base.Interfaces;
using Krypton.Budgets.Domain._Base.Services;
using Krypton.Budgets.Domain._Impl.Interfaces;
using Microsoft.Extensions.Logging;

namespace Krypton.Budgets.Domain.Users.User_.DeleteUser;

internal class DeleteUser : BaseOperation<DeleteUser, ITargetArgs, IOpResults>, IDeleteUser
{
    public DeleteUser(IContext context, ILogger<DeleteUser> logger) : base(context, logger) { }

    protected override void AssertPermissions() => AssertIsAdmin();

    protected override async Task<V> InnerExecute<V>(ITargetArgs? args, Func<IOpResults, V> resultsFactory)
    {
        var user = await GetTarget<User>(args);

        await user.DeleteAsync();

        await context.Persistence.FlushChangesAsync();

        return resultsFactory(EmptyResults.Instance);
    }
}

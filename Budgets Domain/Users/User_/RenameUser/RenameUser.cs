using Krypton.Budgets.Domain._Base.Interfaces;
using Krypton.Budgets.Domain._Base.Services;
using Krypton.Budgets.Domain._Impl.Interfaces;
using Microsoft.Extensions.Logging;

namespace Krypton.Budgets.Domain.Users.User_.RenameUser;

internal class RenameUser : BaseOperation<RenameUser, IRenameUserArgs, IOpResults>, IRenameUser
{
    public RenameUser(IContext context, ILogger<RenameUser> logger) : base(context, logger) { }

    protected override void AssertPermissions() => AssertIsAdmin();

    protected override async Task<V> InnerExecute<V>(IRenameUserArgs? args, Func<IOpResults, V> resultsFactory)
    {
        var newName = args?.NewName?.Trim() ?? throw BuildUnexpectedValidationFailure();

        var user = await GetTarget<User>(args);

        user.RenameUser(newName);

        await context.Persistence.FlushChangesAsync();

        return resultsFactory(EmptyResults.Instance);
    }
}

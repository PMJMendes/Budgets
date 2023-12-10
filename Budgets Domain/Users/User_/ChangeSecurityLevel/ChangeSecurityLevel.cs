using Krypton.Budgets.Domain._Base.Interfaces;
using Krypton.Budgets.Domain._Base.Services;
using Krypton.Budgets.Domain._Impl.Interfaces;
using Microsoft.Extensions.Logging;

namespace Krypton.Budgets.Domain.Users.User_.ChangeSecurityLevel;

internal class ChangeSecurityLevel : BaseOperation<ChangeSecurityLevel, IChangeSecurityLevelArgs, IOpResults>, IChangeSecurityLevel
{
    public ChangeSecurityLevel(IContext context, ILogger<ChangeSecurityLevel> logger) : base(context, logger) { }

    protected override void AssertPermissions() => AssertIsAdmin();

    protected override async Task<V> InnerExecute<V>(IChangeSecurityLevelArgs? args, Func<IOpResults, V> resultsFactory)
    {
        var newLevel = args?.NewLevel ?? throw BuildUnexpectedValidationFailure();

        var user = await GetTarget<User>(args);

        await user.ChangeSecurityLevelAsync(newLevel);

        await context.Persistence.FlushChangesAsync();

        return resultsFactory(EmptyResults.Instance);
    }
}

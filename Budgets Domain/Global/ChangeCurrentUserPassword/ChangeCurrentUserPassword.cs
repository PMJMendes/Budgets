using Krypton.Budgets.Domain._Base.Interfaces;
using Krypton.Budgets.Domain._Base.Services;
using Krypton.Budgets.Domain._Impl.Interfaces;
using Microsoft.Extensions.Logging;

namespace Krypton.Budgets.Domain.Global.ChangeCurrentUserPassword;

internal class ChangeCurrentUserPassword : BaseOperation<ChangeCurrentUserPassword, IArguments, IOpResults>, IChangeCurrentUserPassword
{
    public ChangeCurrentUserPassword(IContext context, ILogger<ChangeCurrentUserPassword> logger) : base(context, logger) { }

    protected override void AssertPermissions() => AssertIsLoggedIn();

    protected override async Task<V> InnerExecute<V>(IArguments? args, Func<IOpResults, V> resultsFactory)
    {
        await context.GlobalObject.ChangeCurrentUserPasswordAsync();

        await context.Persistence.FlushChangesAsync();

        return resultsFactory(EmptyResults.Instance);
    }
}

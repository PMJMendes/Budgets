using Krypton.Budgets.Domain._Base.Interfaces;
using Krypton.Budgets.Domain._Base.Services;
using Krypton.Budgets.Domain._Impl.Interfaces;
using Krypton.Budgets.Domain.Users.User_;
using Microsoft.Extensions.Logging;

namespace Krypton.Budgets.Domain.Budgets.Budget_.AssignManager;

internal class AssignManager : BaseOperation<AssignManager, IAssignManagerArgs, IOpResults>, IAssignManager
{
    public AssignManager(IContext context, ILogger<AssignManager> logger) : base(context, logger) { }

    protected override void AssertPermissions() => AssertIsProducer();

    protected override async Task<V> InnerExecute<V>(IAssignManagerArgs? args, Func<IOpResults, V> resultsFactory)
    {
        var newManager = args?.NewManagerId is Guid userId ?
            await context.Persistence.GetAsync<User>(userId) :
            null;

        var budget = await GetTarget<Budget>(args);

        budget.AssignTo(newManager);

        await context.Persistence.FlushChangesAsync();

        return resultsFactory(EmptyResults.Instance);
    }
}

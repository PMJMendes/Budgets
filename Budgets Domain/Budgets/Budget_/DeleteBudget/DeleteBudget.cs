using Krypton.Budgets.Domain._Base.Interfaces;
using Krypton.Budgets.Domain._Base.Services;
using Krypton.Budgets.Domain._Impl.Interfaces;
using Microsoft.Extensions.Logging;

namespace Krypton.Budgets.Domain.Budgets.Budget_.DeleteBudget;

internal class DeleteBudget : BaseOperation<DeleteBudget, ITargetArgs, IOpResults>, IDeleteBudget
{
    public DeleteBudget(IContext context, ILogger<DeleteBudget> logger) : base(context, logger) { }

    protected override void AssertPermissions() => AssertIsProducer();

    protected override async Task<V> InnerExecute<V>(ITargetArgs? args, Func<IOpResults, V> resultsFactory)
    {
        var budget = await GetTarget<Budget>(args);

        await budget.DeleteAsync();

        await context.Persistence.FlushChangesAsync();

        return resultsFactory(EmptyResults.Instance);
    }
}

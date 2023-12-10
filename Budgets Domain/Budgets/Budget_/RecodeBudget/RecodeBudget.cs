using Krypton.Budgets.Domain._Base.Interfaces;
using Krypton.Budgets.Domain._Base.Services;
using Krypton.Budgets.Domain._Impl.Interfaces;
using Microsoft.Extensions.Logging;

namespace Krypton.Budgets.Domain.Budgets.Budget_.RecodeBudget;

internal class RecodeBudget : BaseOperation<RecodeBudget, IRecodeBudgetArgs, IOpResults>, IRecodeBudget
{
    public RecodeBudget(IContext context, ILogger<RecodeBudget> logger) : base(context, logger) { }

    protected override void AssertPermissions() => AssertIsProducer();

    protected override async Task<V> InnerExecute<V>(IRecodeBudgetArgs? args, Func<IOpResults, V> resultsFactory)
    {
        var newCode = args?.NewCode ?? throw BuildUnexpectedValidationFailure();

        var budget = await GetTarget<Budget>(args);

        budget.Recode(newCode);

        await context.Persistence.FlushChangesAsync();

        return resultsFactory(EmptyResults.Instance);
    }
}

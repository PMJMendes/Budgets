using Krypton.Budgets.Domain._Base.Interfaces;
using Krypton.Budgets.Domain._Base.Services;
using Krypton.Budgets.Domain._Impl.Exceptions;
using Krypton.Budgets.Domain._Impl.Interfaces;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace Krypton.Budgets.Domain.Budgets.Budget_.ManageBudget;

internal class ManageBudget : BaseOperation<ManageBudget, IManageBudgetArgs, IOpResults>, IManageBudget
{
    public ManageBudget(IContext context, ILogger<ManageBudget> logger) : base(context, logger) { }

    protected override void AssertPermissions() => AssertIsLoggedIn();

    protected override async Task<V> InnerExecute<V>(IManageBudgetArgs? args, Func<IOpResults, V> resultsFactory)
    {
        Exception ex = BuildUnexpectedValidationFailure();

        var budgetData = args?.ToBudgetData(ex) ?? throw ex;
        var nWeatherDays = args?.NWeatherDays;

        var budget = await GetTarget<Budget>(args);

        if (budget.State != BudgetState.LOCKED || budget.Manager != context.CurrentUser)
        {
            AssertIsProducer();
        }

        try
        {
            await budget.UpdateManagementAsync(nWeatherDays, budgetData);
        }
        catch (ConsistencyException e) when (e.Errors.FirstOrDefault() is (Enum, PropertyInfo) err)
        {
            throw new InvalidArgsException(err.PropRef, err.Tag, e.Message);
        }

        await context.Persistence.FlushChangesAsync();

        return resultsFactory(EmptyResults.Instance);
    }
}

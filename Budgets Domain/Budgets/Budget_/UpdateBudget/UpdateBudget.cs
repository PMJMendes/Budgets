using Krypton.Budgets.Domain._Base.Interfaces;
using Krypton.Budgets.Domain._Base.Services;
using Krypton.Budgets.Domain._Impl.Exceptions;
using Krypton.Budgets.Domain._Impl.Interfaces;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace Krypton.Budgets.Domain.Budgets.Budget_.UpdateBudget;

internal class UpdateBudget : BaseOperation<UpdateBudget, IUpdateBudgetArgs, IOpResults>, IUpdateBudget
{
    public UpdateBudget(IContext context, ILogger<UpdateBudget> logger) : base(context, logger) { }

    protected override void AssertPermissions() => AssertIsProducer();

    protected override async Task<V> InnerExecute<V>(IUpdateBudgetArgs? args, Func<IOpResults, V> resultsFactory)
    {
        Exception ex = BuildUnexpectedValidationFailure();

        var frontData = args?.FrontData?.ToFrontData(ex) ?? throw ex;
        var finalData = args?.FinalData?.ToFinalData() ?? throw ex;
        var defData = args?.ToDefData(ex) ?? throw ex;
        var budgetData = args?.ToBudgetData(ex) ?? throw ex;

        var budget = await GetTarget<Budget>(args);

        try
        {
            budget.UpdateFrontData(frontData);
            budget.UpdateFinalData(finalData);
            await budget.UpdateDefinitionAsync(defData);
            await budget.UpdateDataAsync(budgetData);
        }
        catch (ConsistencyException e) when (e.Errors.FirstOrDefault() is (Enum, PropertyInfo) err)
        {
            throw new InvalidArgsException(err.PropRef, err.Tag, e.Message);
        }

        await context.Persistence.FlushChangesAsync();

        return resultsFactory(EmptyResults.Instance);
    }
}

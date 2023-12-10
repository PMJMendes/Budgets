using Krypton.Budgets.Domain.Global.NextBudgetCode;

namespace Krypton.Budgets.API.Global.NextBudgetCode;

internal readonly struct NextBudgetCodeItem : INextBudgetCodeItem
{
    public NextBudgetCodeItem(INextBudgetCodeItem source)
    {
        NextCode = source.NextCode;
    }

    public string NextCode { get; private init; }
}

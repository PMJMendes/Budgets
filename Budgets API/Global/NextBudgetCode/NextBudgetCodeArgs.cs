using Krypton.Budgets.Domain.Global.NextBudgetCode;

namespace Krypton.Budgets.API.Global.NextBudgetCode;

internal struct NextBudgetCodeArgs : INextBudgetCodeArgs
{
    public string? Prefix { get; set; }
}

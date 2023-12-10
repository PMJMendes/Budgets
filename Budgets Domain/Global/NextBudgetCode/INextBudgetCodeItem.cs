using Krypton.Budgets.Domain._Base.Interfaces;

namespace Krypton.Budgets.Domain.Global.NextBudgetCode;

public interface INextBudgetCodeItem : IQueryResultItem
{
    string NextCode { get; }
}

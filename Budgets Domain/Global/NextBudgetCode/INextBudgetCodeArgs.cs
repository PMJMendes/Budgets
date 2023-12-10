using Krypton.Budgets.Domain._Base.Interfaces;

namespace Krypton.Budgets.Domain.Global.NextBudgetCode;

public interface INextBudgetCodeArgs : IArguments
{
    string? Prefix { get; }
}

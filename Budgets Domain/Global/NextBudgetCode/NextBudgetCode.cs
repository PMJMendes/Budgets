using Krypton.Budgets.Domain._Base.Services;
using Krypton.Budgets.Domain._Impl.Interfaces;
using Krypton.Budgets.Domain._Impl.Utils;
using Krypton.Budgets.Domain.Budgets.Budget_;
using Microsoft.Extensions.Logging;

namespace Krypton.Budgets.Domain.Global.NextBudgetCode;

internal class NextBudgetCode : BaseQuery<NextBudgetCode, INextBudgetCodeArgs, INextBudgetCodeItem>, INextBudgetCode
{
    public NextBudgetCode(IContext context, ILogger<NextBudgetCode> logger) : base(context, logger) { }

    protected override void AssertPermissions() => AssertIsProducer();

    protected override IAsyncEnumerable<V> InnerFetch<V>(INextBudgetCodeArgs? args, Func<INextBudgetCodeItem, V> itemFactory)
    {
        var prefix = args?.Prefix ?? (DateTime.Now.ToString("yyyyMM") + ".");

        var lastCode = context.Persistence.Query<Budget>().
            Where(b => b.Code.StartsWith(prefix)).
            OrderByDescending(b => b.Code).
            Select(b => b.Code.Substring(prefix.Length)).
            Take(1).
            AsEnumerable().
            Select(s => (IsNum: int.TryParse(s, out var num), Val: num)).
            Where(p => p.IsNum).
            Select(p => p.Val).
            FirstOrDefault();

        return itemFactory(new Item(prefix + (lastCode + 1).ToString("D3"))).
            AsSingleEnumerator().
            ToAsyncEnumerable();
    }

    private record struct Item(
        string NextCode
    ) : INextBudgetCodeItem;
}

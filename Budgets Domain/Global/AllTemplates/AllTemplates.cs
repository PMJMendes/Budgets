using Krypton.Budgets.Domain._Base.Interfaces;
using Krypton.Budgets.Domain._Base.Services;
using Krypton.Budgets.Domain._Impl.Interfaces;
using Krypton.Budgets.Domain.Budgets.Budget_;
using Microsoft.Extensions.Logging;

namespace Krypton.Budgets.Domain.Global.AllTemplates;

internal class AllTemplates : BaseQuery<AllTemplates, IArguments, IAllTemplatesItem>, IAllTemplates
{
    public AllTemplates(IContext context, ILogger<AllTemplates> logger) : base(context, logger) { }

    protected override void AssertPermissions() => AssertIsProducer();

    protected override IAsyncEnumerable<V> InnerFetch<V>(IArguments? args, Func<IAllTemplatesItem, V> itemFactory)
    {
        return context.Persistence.Query<Budget>().
            Where(b => b.IsTemplate).
            ToAsyncEnumerable().
            Select(b => itemFactory(new Item(
                b.Id,
                b.Code,
                b.Title
            )));
    }

    private record struct Item(
        Guid Id,
        string Code,
        string Title
    ) : IAllTemplatesItem;
}

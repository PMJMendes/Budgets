using Krypton.Budgets.API._Base;
using Krypton.Budgets.API._Impl;
using Krypton.Budgets.Domain.Global.AllTemplates;

namespace Krypton.Budgets.API.Global.AllTemplates;

internal class AllTemplates : GetEndpointBase<AllTemplates, AllTemplatesItem>
{
    private readonly IAllTemplates query;

    public AllTemplates(IAllTemplates query, IHttpContextAccessor accessor, ILogger<AllTemplates> logger) : base(accessor, logger)
    {
        this.query = query;
    }

    protected override Task<IResult> GetAsync() => InRequestScope(() =>
        CallDomainQueryAsync(query, EmptyArgs.Instance, r => new AllTemplatesItem(r)));
}

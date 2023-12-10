using Krypton.Budgets.API._Base;
using Krypton.Budgets.API._Impl;
using Krypton.Budgets.Domain.Global.AllUsers;

namespace Krypton.Budgets.API.Global.AllUsers;

internal class AllUsers : GetEndpointBase<AllUsers, AllUsersItem>
{
    private readonly IAllUsers query;

    public AllUsers(IAllUsers query, IHttpContextAccessor accessor, ILogger<AllUsers> logger) : base(accessor, logger)
    {
        this.query = query;
    }

    protected override Task<IResult> GetAsync() => InRequestScope(() =>
        CallDomainQueryAsync(query, EmptyArgs.Instance, r => new AllUsersItem(r)));
}

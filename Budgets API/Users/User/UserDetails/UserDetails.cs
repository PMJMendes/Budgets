using Krypton.Budgets.API._Base;
using Krypton.Budgets.API._Impl;
using Krypton.Budgets.Domain.Users.User_.UserDetails;

namespace Krypton.Budgets.API.Users.User.UserDetails;

internal class UserDetails : GetEndpointBase<UserDetails, UserDetailsItem, TargetQueryArgs>
{
    private readonly IUserDetails query;

    public UserDetails(IUserDetails query, IHttpContextAccessor accessor, ILogger<UserDetails> logger) : base(accessor, logger)
    {
        this.query = query;
    }

    protected override Task<IResult> GetAsync(TargetQueryArgs args) => InRequestScope(() =>
        CallDomainQueryAsync(query, args, r => new UserDetailsItem(r)));
}

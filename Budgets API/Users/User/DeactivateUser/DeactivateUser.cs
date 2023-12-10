using Krypton.Budgets.API._Base;
using Krypton.Budgets.API._Impl;
using Krypton.Budgets.Domain.Users.User_.DeactivateUser;

namespace Krypton.Budgets.API.Users.User.DeactivateUser;

internal class DeactivateUser : PostEndpointBase<DeactivateUser, DeactivateUserResults, TargetArgs>
{
    private readonly IDeactivateUser op;

    public DeactivateUser(IDeactivateUser op, IHttpContextAccessor accessor, ILogger<DeactivateUser> logger) : base(accessor, logger)
    {
        this.op = op;
    }

    protected override Task<IResult> PostAsync(TargetArgs? args) => InRequestScope(() =>
        CallDomainOpAsync(op, args, r => new DeactivateUserResults(r)));
}

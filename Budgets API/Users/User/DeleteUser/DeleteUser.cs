using Krypton.Budgets.API._Base;
using Krypton.Budgets.API._Impl;
using Krypton.Budgets.Domain.Users.User_.DeleteUser;

namespace Krypton.Budgets.API.Users.User.DeleteUser;

internal class DeleteUser : PostEndpointBase<DeleteUser, EmptyResults, TargetArgs>
{
    private readonly IDeleteUser op;

    public DeleteUser(IDeleteUser op, IHttpContextAccessor accessor, ILogger<DeleteUser> logger) : base(accessor, logger)
    {
        this.op = op;
    }

    protected override Task<IResult> PostAsync(TargetArgs? args) => InRequestScope(() =>
        CallDomainOpAsync(op, args, _ => EmptyResults.Instance));
}

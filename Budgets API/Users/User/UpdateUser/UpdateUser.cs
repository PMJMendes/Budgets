using Krypton.Budgets.API._Base;
using Krypton.Budgets.API._Impl;
using Krypton.Budgets.Domain.Users.User_.ChangeEmail;
using Krypton.Budgets.Domain.Users.User_.ChangeSecurityLevel;
using Krypton.Budgets.Domain.Users.User_.RenameUser;

namespace Krypton.Budgets.API.Users.User.UpdateUser;

internal class UpdateUser : PostEndpointBase<UpdateUser, UpdateUserResults, UpdateUserArgs>
{
    private readonly IChangeEmail opChangeEmail;
    private readonly IChangeSecurityLevel opChangeSecurityLevel;
    private readonly IRenameUser opRenameUser;

    public UpdateUser(IChangeEmail opChangeEmail, IChangeSecurityLevel opChangeSecurityLevel, IRenameUser opRenameUser,
        IHttpContextAccessor accessor, ILogger<UpdateUser> logger) : base(accessor, logger)
    {
        this.opChangeEmail = opChangeEmail;
        this.opChangeSecurityLevel = opChangeSecurityLevel;
        this.opRenameUser = opRenameUser;
    }

    protected override async Task<IResult> PostAsync(UpdateUserArgs? args) => await InRequestScope(async () =>
    {
        var result = await CallDomainOpAsync(opChangeEmail, args, r => new UpdateUserResults(r, args));

        await CallDomainOpAsync(opChangeSecurityLevel, args, _ => EmptyResults.Instance);

        await CallDomainOpAsync(opRenameUser, args, _ => EmptyResults.Instance);

        return result;
    });
}

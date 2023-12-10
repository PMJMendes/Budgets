using Krypton.Budgets.API._Base;
using Krypton.Budgets.Domain.Global.CreateUser;

namespace Krypton.Budgets.API.Global.CreateUser;

internal class CreateUser : PostEndpointBase<CreateUser, CreateUserResults, CreateUserArgs>
{
    private readonly ICreateUser op;

    public CreateUser(ICreateUser op, IHttpContextAccessor accessor, ILogger<CreateUser> logger) : base(accessor, logger)
    {
        this.op = op;
    }

    protected override Task<IResult> PostAsync(CreateUserArgs? args) => InRequestScope(() =>
        CallDomainOpAsync(op, args, r => new CreateUserResults(r, args)));
}

using Krypton.Budgets.Domain._Base.Services;
using Krypton.Budgets.Domain._Impl.Interfaces;
using Krypton.Budgets.Domain.Users.User_;
using Microsoft.Extensions.Logging;

namespace Krypton.Budgets.Domain.Global.CreateUser;

internal class CreateUser : BaseOperation<CreateUser, ICreateUserArgs, ICreateUserResults>, ICreateUser
{
    public CreateUser(IContext context, ILogger<CreateUser> logger) : base(context, logger) { }

    protected override void AssertPermissions() => AssertIsAdmin();

    protected override async Task<V> InnerExecute<V>(ICreateUserArgs? args, Func<ICreateUserResults, V> resultsFactory)
    {
        var email = args?.Email?.Trim() ?? throw BuildUnexpectedValidationFailure();
        var level = args?.Level ?? throw BuildUnexpectedValidationFailure();
        var fullName = args?.FullName?.Trim() ?? throw BuildUnexpectedValidationFailure();

        var newUser = context.GlobalObject.CreateUser(email, level, fullName);

        await context.Persistence.FlushChangesAsync();

        return resultsFactory(new Result(
            newUser.Id,
            newUser.State
        ));
    }

    private record struct Result(
        Guid NewUserId,
        UserState State
    ) : ICreateUserResults;
}

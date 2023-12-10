using Krypton.Budgets.Domain._Base.Services;
using Krypton.Budgets.Domain._Impl.Interfaces;
using Microsoft.Extensions.Logging;

namespace Krypton.Budgets.Domain.Users.User_.ChangeEmail;

internal class ChangeEmail : BaseOperation<ChangeEmail, IChangeEmailArgs, IChangeEmailResults>, IChangeEmail
{
    public ChangeEmail(IContext context, ILogger<ChangeEmail> logger) : base(context, logger) { }

    protected override void AssertPermissions() => AssertIsAdmin();

    protected override async Task<V> InnerExecute<V>(IChangeEmailArgs? args, Func<IChangeEmailResults, V> resultsFactory)
    {
        var newEmail = args?.NewEmail?.Trim() ?? throw BuildUnexpectedValidationFailure();

        var user = await GetTarget<User>(args);

        await user.ChangeEmailAsync(newEmail);

        await context.Persistence.FlushChangesAsync();

        return resultsFactory(new Result(
            user.State
        ));
    }

    private record struct Result(
        UserState State
    ) : IChangeEmailResults;
}

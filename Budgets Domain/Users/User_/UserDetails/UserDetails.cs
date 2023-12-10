using Krypton.Budgets.Domain._Base.Interfaces;
using Krypton.Budgets.Domain._Base.Services;
using Krypton.Budgets.Domain._Impl.Interfaces;
using Krypton.Budgets.Domain._Ports.Security;
using Microsoft.Extensions.Logging;

namespace Krypton.Budgets.Domain.Users.User_.UserDetails;

internal class UserDetails : BaseQuery<UserDetails, ITargetArgs, IUserDetailsItem>, IUserDetails
{
    public UserDetails(IContext context, ILogger<UserDetails> logger) : base(context, logger) { }

    protected override void AssertPermissions() => AssertIsAdmin();

    protected override async IAsyncEnumerable<V> InnerFetch<V>(ITargetArgs? args, Func<IUserDetailsItem, V> itemFactory)
    {
        var user = await GetTarget<User>(args);

        yield return itemFactory(new Item(
            user.Id,
            user.Email,
            user.Level,
            user.FullName,
            user.State
        ));
    }

    private record struct Item(
        Guid Id,
        string Email,
        SecurityLevel Level,
        string FullName,
        UserState State
    ) : IUserDetailsItem;
}

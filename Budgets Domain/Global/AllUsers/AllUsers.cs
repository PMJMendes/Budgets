using Krypton.Budgets.Domain._Base.Interfaces;
using Krypton.Budgets.Domain._Base.Services;
using Krypton.Budgets.Domain._Impl.Interfaces;
using Krypton.Budgets.Domain._Ports.Security;
using Krypton.Budgets.Domain.Users.User_;
using Microsoft.Extensions.Logging;

namespace Krypton.Budgets.Domain.Global.AllUsers;

internal class AllUsers : BaseQuery<AllUsers, IArguments, IAllUsersItem>, IAllUsers
{
    public AllUsers(IContext context, ILogger<AllUsers> logger) : base(context, logger) { }

    protected override void AssertPermissions() => AssertIsAdmin();

    protected override IAsyncEnumerable<V> InnerFetch<V>(IArguments? args, Func<IAllUsersItem, V> itemFactory)
    {
        return context.Persistence.Query<User>().
            ToAsyncEnumerable().
            Select(u => itemFactory(new Item(
                u.Id,
                u.Email,
                u.Level,
                u.FullName,
                u.State
            )));
    }

    private record struct Item
    (
        Guid Id,
        string Email,
        SecurityLevel Level,
        string FullName,
        UserState State
    ) : IAllUsersItem;
}

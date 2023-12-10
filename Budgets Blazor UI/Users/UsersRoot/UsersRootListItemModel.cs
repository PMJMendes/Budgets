using Krypton.Budgets.Blazor.APIClient.Global.AllUsers;
using Krypton.Budgets.Blazor.UI.Users._Common;

namespace Krypton.Budgets.Blazor.UI.Users.UsersRoot;

public class UsersRootListItemModel
{
    private UsersRootListItemModel(
        Guid id,
        string email,
        SecurityLevel level,
        UserState state)
    {
        Id = id;
        Email = email;
        Level = level;
        State = state;
    }

    public Guid Id { get; private init; }
    public string Email { get; private init; }
    public SecurityLevel Level { get; private init; }
    public UserState State { get; private init; }

    public static UsersRootListItemModel Empty() => new(
        Guid.Empty,
        "",
        SecurityLevel._UNKNOWN,
        UserState._UNKNOWN
    );

    public static UsersRootListItemModel FromService(AllUsersItem item)
    {
        return new(
            item.Id ?? Guid.Empty,
            item.Email ?? "",
            Enum.TryParse(item.Level, true, out SecurityLevel level) ? level : SecurityLevel._UNKNOWN,
            Enum.TryParse(item.State, true, out UserState state) ? state : UserState._UNKNOWN
        );
    }
}

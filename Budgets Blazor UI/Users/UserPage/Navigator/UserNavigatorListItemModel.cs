using Krypton.Budgets.Blazor.APIClient.Global.AllUsers;
using Krypton.Budgets.Blazor.APIClient.Global.CreateUser;
using Krypton.Budgets.Blazor.APIClient.Users.User.UpdateUser;
using Krypton.Budgets.Blazor.UI.Users._Common;
using Krypton.Budgets.Blazor.UI.Users.UsersRoot;

namespace Krypton.Budgets.Blazor.UI.Users.UserPage.Navigator;

public class UserNavigatorListItemModel
{
	private UserNavigatorListItemModel(
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

	public UserNavigatorListItemModel WithState(UserState state) => new(
		Id,
		Email,
		Level,
		state
	);

	public static UserNavigatorListItemModel Empty() => new(
		Guid.Empty,
		"",
		SecurityLevel._UNKNOWN,
		UserState._UNKNOWN
	);

	public static UserNavigatorListItemModel FromRoot(UsersRootListItemModel rootModel) => new(
		rootModel.Id,
		rootModel.Email,
		rootModel.Level,
		rootModel.State
	);

	public static UserNavigatorListItemModel FromService(AllUsersItem item)
	{
		return new(
			item.Id ?? Guid.Empty,
			item.Email ?? "",
			Enum.TryParse(item.Level, true, out SecurityLevel level) ? level : SecurityLevel._UNKNOWN,
			Enum.TryParse(item.State, true, out UserState state) ? state : UserState._UNKNOWN
		);
	}

	public static UserNavigatorListItemModel FromCreateResults(CreateUserResults? results) => new(
		results?.NewUserId ?? Guid.Empty,
		results?.Email ?? "",
		Enum.TryParse(results?.Level, true, out SecurityLevel level) ? level : SecurityLevel._UNKNOWN,
		Enum.TryParse(results?.State, true, out UserState state) ? state : UserState._UNKNOWN
	);

	public static UserNavigatorListItemModel FromEditResults(UpdateUserResults? results) => new(
		results?.Id ?? Guid.Empty,
		results?.Email ?? "",
		Enum.TryParse(results?.Level, true, out SecurityLevel level) ? level : SecurityLevel._UNKNOWN,
		Enum.TryParse(results?.State, true, out UserState state) ? state : UserState._UNKNOWN
	);
}

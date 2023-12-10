using Krypton.Budgets.Blazor.APIClient.Global.CreateUser;
using Krypton.Budgets.Blazor.APIClient.Users.User.UpdateUser;
using Krypton.Budgets.Blazor.APIClient.Users.User.UserDetails;
using Krypton.Budgets.Blazor.UI.Users._Common;
using Krypton.Budgets.Blazor.UI.Users.UserPage._Core;

namespace Krypton.Budgets.Blazor.UI.Users.UserPage.View;

public class UserViewModel
{
	public UserViewModel(Guid id, UserModel userData, UserState state)
	{
		Id = id;
		UserData = userData;
		State = state;
	}

	public Guid Id { get; private init; }
	public UserModel UserData { get; private init; }
	public UserState State { get; private init; }

	public UserViewModel WithState(UserState state) => new(
		Id,
		UserData,
		state
	);

	public static UserViewModel Empty() => new(
		Guid.Empty,
		UserModel.Empty(),
		UserState._UNKNOWN
	);

	public static UserViewModel FromService(UserDetailsItem item) => new(
		item.Id ?? Guid.Empty,
		UserModel.FromService(item),
		Enum.TryParse(item?.State, true, out UserState state) ? state : UserState._UNKNOWN
	);

	public static UserViewModel FromCreateResults(CreateUserResults? results) => new(
		results?.NewUserId ?? Guid.Empty,
		UserModel.FromCreateResults(results),
		Enum.TryParse(results?.State, true, out UserState state) ? state : UserState._UNKNOWN
	);

	public static UserViewModel FromUpdateResults(UpdateUserResults? results) => new(
		results?.Id ?? Guid.Empty,
		UserModel.FromEditResults(results),
		Enum.TryParse(results?.State, true, out UserState state) ? state : UserState._UNKNOWN
	);
}

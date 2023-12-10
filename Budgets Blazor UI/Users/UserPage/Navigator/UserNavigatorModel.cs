using Krypton.Budgets.Blazor.APIClient.Global.AllUsers;
using Krypton.Budgets.Blazor.APIClient.Global.CreateUser;
using Krypton.Budgets.Blazor.APIClient.Users.User.UpdateUser;
using Krypton.Budgets.Blazor.UI.Users._Common;
using Krypton.Budgets.Blazor.UI.Users.UsersRoot;

namespace Krypton.Budgets.Blazor.UI.Users.UserPage.Navigator;

public class UserNavigatorModel
{
	private UserNavigatorModel(
		IEnumerable<UserNavigatorListItemModel> users,
		Guid selectedId)
	{
		Users = users;
		SelectedId = selectedId;
	}

	public IEnumerable<UserNavigatorListItemModel> Users { get; private init; }
	public Guid SelectedId { get; private init; }

	public UserNavigatorModel WithId(Guid id) => new(
		Users,
		id
	);

	public UserNavigatorModel WithResults(IEnumerable<AllUsersItem?>? items) => new(
		(items ?? Array.Empty<AllUsersItem>()).
			Where(i => i is not null).
			Select(i => UserNavigatorListItemModel.FromService(i!)).
			ToList(),
		SelectedId
	);

	public UserNavigatorModel WithCreateResults(CreateUserResults? results) => new(
		Users.Prepend(UserNavigatorListItemModel.FromCreateResults(results)).ToList(),
		results?.NewUserId ?? Guid.Empty
	);

	public UserNavigatorModel WithEditResults(UpdateUserResults? results) => new(
		Users.Select(c => c.Id == (results?.Id ?? Guid.Empty) ? UserNavigatorListItemModel.FromEditResults(results) : c).ToList(),
		results?.Id ?? Guid.Empty
	);

	public UserNavigatorModel WithSelectedUserState(UserState state) => new(
		Users.Select(c => c.Id == SelectedId ? c.WithState(state) : c).ToList(),
		SelectedId
	);

	public static UserNavigatorModel Empty() => new(
		Array.Empty<UserNavigatorListItemModel>(),
		Guid.Empty
	);

	public static UserNavigatorModel FromRoot(UsersRootModel rootModel) => new(
		rootModel.Items.
			Select(i => UserNavigatorListItemModel.FromRoot(i)).
			ToList(),
		Guid.Empty
	);
}

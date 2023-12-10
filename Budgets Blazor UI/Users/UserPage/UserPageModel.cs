using Krypton.Budgets.Blazor.APIClient.Global.CreateUser;
using Krypton.Budgets.Blazor.APIClient.Users.User.UpdateUser;
using Krypton.Budgets.Blazor.UI.Users._Common;
using Krypton.Budgets.Blazor.UI.Users.UserPage.Create;
using Krypton.Budgets.Blazor.UI.Users.UserPage.Edit;
using Krypton.Budgets.Blazor.UI.Users.UserPage.Navigator;
using Krypton.Budgets.Blazor.UI.Users.UserPage.View;
using Krypton.Budgets.Blazor.UI.Users.UsersRoot;

namespace Krypton.Budgets.Blazor.UI.Users.UserPage;

public class UserPageModel
{
	private UserPageModel(bool empty, UserNavigatorModel navData,
		UserCreateModel? createData = null, UserViewModel? viewData = null, UserEditModel? editData = null)
	{
		IsNew = empty;
		NavData = navData;

		CreateData = createData;
		ViewData = viewData;
		EditData = editData;
	}

	public bool IsNew { get; private init; }
	public UserNavigatorModel NavData { get; private init; }
	public UserCreateModel? CreateData { get; private init; }
	public UserViewModel? ViewData { get; private init; }
	public UserEditModel? EditData { get; private init; }

	public UserPageModel ForCreate() => new(
		IsNew,
		NavData.WithId(Guid.Empty),
		UserCreateModel.Empty()
	);

	public UserPageModel WithCreateResults(CreateUserResults? results) => new(
		IsNew,
		NavData.WithCreateResults(results),
		null,
		UserViewModel.FromCreateResults(results)
	);

	public UserPageModel ForView(Guid id) => new(
		IsNew,
		NavData.WithId(id)
	);

	public UserPageModel ForEdit() => new(
		IsNew,
		NavData,
		null,
		ViewData,
		UserEditModel.FromViewData(ViewData ?? UserViewModel.Empty())
	);

	public UserPageModel WithEditResults(UpdateUserResults? results) => new(
		IsNew,
		NavData.WithEditResults(results),
		null,
		UserViewModel.FromUpdateResults(results)
	);

	public UserPageModel CancelEdit() => new(
		IsNew,
		NavData,
		null,
		ViewData
	);

	public UserPageModel WithSelectedUserState(UserState state) => new(
		IsNew,
		NavData.WithSelectedUserState(state),
		CreateData,
		ViewData?.WithState(state),
		EditData
	);

	public static UserPageModel Empty() => new(
		true,
		UserNavigatorModel.Empty()
	);

	public static UserPageModel FromRoot(UsersRootModel rootModel, bool forCreate) => new(
		false,
		UserNavigatorModel.FromRoot(rootModel),
		forCreate ? UserCreateModel.Empty() : null
	);

	public static UserPageModel FromResolver(UserNavigatorModel navData, UserCreateModel? createData,
		UserViewModel? viewData = null) => new(
		false,
		navData,
		createData,
		viewData
	);
}

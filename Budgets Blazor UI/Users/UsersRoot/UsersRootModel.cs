using Krypton.Budgets.Blazor.APIClient.Global.AllUsers;

namespace Krypton.Budgets.Blazor.UI.Users.UsersRoot;

public class UsersRootModel
{
	private UsersRootModel(
		IEnumerable<UsersRootListItemModel> items)
	{
		Items = items;
	}

	public IEnumerable<UsersRootListItemModel> Items { get; private init; }

	public static UsersRootModel Empty() => new(
		Array.Empty<UsersRootListItemModel>()
	);

	public static UsersRootModel WithResults(IEnumerable<AllUsersItem?>? items) => new(
		(items ?? Array.Empty<AllUsersItem>()).
			Where(i => i is not null).
			Select(i => UsersRootListItemModel.FromService(i!)).
			ToList()
	);
}

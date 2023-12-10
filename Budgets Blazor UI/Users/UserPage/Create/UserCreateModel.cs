using Krypton.Budgets.Blazor.APIClient.Global.CreateUser;
using Krypton.Budgets.Blazor.UI.Users.UserPage._Core;

namespace Krypton.Budgets.Blazor.UI.Users.UserPage.Create;

public class UserCreateModel
{
	private UserCreateModel(UserModel userData)
	{
		UserData = userData;
	}

	public UserModel UserData { get; private init; }

	public CreateUserArgs AsArgs() => new(
		UserData.Email,
		UserData.Level.ToString(),
		UserData.FullName
	);

	public static UserCreateModel Empty() => new(
		UserModel.Empty()
	);
}

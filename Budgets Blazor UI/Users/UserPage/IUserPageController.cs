using Krypton.Budgets.Blazor.APIClient.Global.CreateUser;
using Krypton.Budgets.Blazor.APIClient.Users.User.UpdateUser;
using Krypton.Budgets.Blazor.UI.Users._Common;

namespace Krypton.Budgets.Blazor.UI.Users.UserPage;

public interface IUserPageController
{
	void OnCreate();
	Task AfterCreateAsync(CreateUserResults? results);
	void OnCancelCreate();

	void OnView(Guid id);

	void OnEdit();
	Task AfterEditAsync(UpdateUserResults? results);
	void OnCancelEdit();

	Task OnStateChangedAsync(UserState state);

	Task OnDeleteAsync();
}

using Krypton.Budgets.Blazor.APIClient.Users.User.UpdateUser;
using Krypton.Budgets.Blazor.UI.Users.UserPage._Core;
using Krypton.Budgets.Blazor.UI.Users.UserPage.View;

namespace Krypton.Budgets.Blazor.UI.Users.UserPage.Edit;

public class UserEditModel
{
    private UserEditModel(Guid id, UserModel userData)
    {
        Id = id;
        UserData = userData;
    }

    public Guid Id { get; private init; }
    public UserModel UserData { get; private init; }

    public UpdateUserArgs AsArgs() => new(
        Id,
        UserData.Email,
        UserData.Level.ToString(),
        UserData.FullName
    );

    public static UserEditModel Empty() => new(
        Guid.Empty,
        UserModel.Empty()
    );

    public static UserEditModel FromViewData(UserViewModel viewData) => new(
        viewData.Id,
        viewData.UserData.Clone()
    );
}

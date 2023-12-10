using Krypton.Budgets.Blazor.APIClient.Global.CreateUser;
using Krypton.Budgets.Blazor.APIClient.Users.User.UpdateUser;
using Krypton.Budgets.Blazor.APIClient.Users.User.UserDetails;
using Krypton.Budgets.Blazor.UI.Users._Common;

namespace Krypton.Budgets.Blazor.UI.Users.UserPage._Core;

public class UserModel
{
    private UserModel(string email, SecurityLevel level, string fullName)
    {
        Email = email;
        Level = level;
        FullName = fullName;
    }

    public string Email { get; set; }
    public SecurityLevel Level { get; set; }
    public string FullName { get; set; }

    public UserModel Clone() => new(
        Email,
        Level,
        FullName
    );

    public static UserModel Empty() => new(
        "",
        SecurityLevel._UNKNOWN,
        ""
    );

    public static UserModel FromService(UserDetailsItem item) => new(
        item.Email ?? "",
        Enum.TryParse(item.Level, true, out SecurityLevel level) ? level : SecurityLevel._UNKNOWN,
        item.FullName ?? ""
    );

    public static UserModel FromCreateResults(CreateUserResults? results) => new(
    results?.Email ?? "",
        Enum.TryParse(results?.Level, true, out SecurityLevel level) ? level : SecurityLevel._UNKNOWN,
        results?.FullName ?? ""
    );

    public static UserModel FromEditResults(UpdateUserResults? results) => new(
    results?.Email ?? "",
        Enum.TryParse(results?.Level, true, out SecurityLevel level) ? level : SecurityLevel._UNKNOWN,
        results?.FullName ?? ""
    );
}

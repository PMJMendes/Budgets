using Krypton.Budgets.Blazor.APIClient._Base;
using Krypton.Budgets.Blazor.APIClient._Common;

namespace Krypton.Budgets.Blazor.APIClient.Users.User.UpdateUser;

public class UpdateUserService : PostServiceBase<UpdateUserArgs, UpdateUserResults>
{
    private static readonly string URL = "users/UpdateUser";

    public UpdateUserService(HttpClient client) : base(client, URL) { }

    public async Task<SafeResult<UpdateUserResults>> UpdateUserAsync(UpdateUserArgs args) =>
        await PostAsync(args);
}

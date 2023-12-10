using Krypton.Budgets.Blazor.APIClient._Base;
using Krypton.Budgets.Blazor.APIClient._Common;
using Krypton.Budgets.Blazor.APIClient._Impl;

namespace Krypton.Budgets.Blazor.APIClient.Users.User.DeleteUser;

public class DeleteUserService : PostServiceBase<TargetPostArgs, EmptyResults>
{
    private static readonly string URL = "users/DeleteUser";

    public DeleteUserService(HttpClient client) : base(client, URL) { }

    public async Task<SafeResult<EmptyResults>> DeleteUserAsync(TargetPostArgs args) =>
        await PostAsync(args);
}

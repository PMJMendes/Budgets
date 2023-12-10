using Krypton.Budgets.Blazor.APIClient._Base;
using Krypton.Budgets.Blazor.APIClient._Common;

namespace Krypton.Budgets.Blazor.APIClient.Users.User.DeactivateUser;

public class DeactivateUserService : PostServiceBase<TargetPostArgs, DeactivateUserResults>
{
    private static readonly string URL = "users/DeactivateUser";

    public DeactivateUserService(HttpClient client) : base(client, URL) { }

    public async Task<SafeResult<DeactivateUserResults>> DeactivateUserAsync(TargetPostArgs args) =>
        await PostAsync(args);
}

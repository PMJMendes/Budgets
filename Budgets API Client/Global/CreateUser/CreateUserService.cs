using Krypton.Budgets.Blazor.APIClient._Base;
using Krypton.Budgets.Blazor.APIClient._Common;

namespace Krypton.Budgets.Blazor.APIClient.Global.CreateUser;

public class CreateUserService : PostServiceBase<CreateUserArgs, CreateUserResults>
{
    private static readonly string URL = "global/CreateUser";

    public CreateUserService(HttpClient client) : base(client, URL) { }

    public async Task<SafeResult<CreateUserResults>> CreateUserAsync(CreateUserArgs args) =>
        await PostAsync(args);
}

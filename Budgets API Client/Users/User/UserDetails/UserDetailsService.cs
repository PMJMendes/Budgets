using Krypton.Budgets.Blazor.APIClient._Base;
using Krypton.Budgets.Blazor.APIClient._Common;

namespace Krypton.Budgets.Blazor.APIClient.Users.User.UserDetails;

public class UserDetailsService : GetServiceBase<TargetQueryArgs, UserDetailsItem>
{
    private static readonly string URL = "users/UserDetails";

    public UserDetailsService(HttpClient client) : base(client, URL) { }

    public async Task<SafeResult<IEnumerable<UserDetailsItem?>>> UserDetailsAsync(TargetQueryArgs args) =>
        await GetAsync(args);
}

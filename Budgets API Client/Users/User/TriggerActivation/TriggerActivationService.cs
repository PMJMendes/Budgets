using Krypton.Budgets.Blazor.APIClient._Base;
using Krypton.Budgets.Blazor.APIClient._Common;

namespace Krypton.Budgets.Blazor.APIClient.Users.User.TriggerActivation;

public class TriggerActivationService : PostServiceBase<TargetPostArgs, TriggerActivationResults>
{
    private static readonly string URL = "users/TriggerActivation";

    public TriggerActivationService(HttpClient client) : base(client, URL) { }

    public async Task<SafeResult<TriggerActivationResults>> TriggerActivationAsync(TargetPostArgs args) =>
        await PostAsync(args);
}

using Krypton.Budgets.Domain._Ports.Hosting;
using Krypton.Budgets.Domain._Ports.Security;
using Microsoft.AspNetCore.Authorization;

namespace Krypton.Budgets.Security.Implementation;

internal class SecurityImpl : ISecurity
{
    private readonly IHost domain;
    private readonly IAuthorizationService auth;

    public SecurityImpl(IHost domain, IAuthorizationService auth)
    {
        this.domain = domain;
        this.auth = auth;
    }

    public bool IsLoggedIn()
    {
        return domain.User.Identity?.IsAuthenticated ?? false;
    }

    public bool HasSecurityLevel(SecurityLevel level)
    {
        return auth.AuthorizeAsync(domain.User, null, level.ToString()).GetAwaiter().GetResult().Succeeded;
    }

    public string LoginId => domain.User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value ?? "(unknown)";

    public string LoginTag
    {
        get
        {
            var claims = domain.User.Claims;

            return claims.FirstOrDefault(c => c.Type == "email")?.Value ??
                claims.FirstOrDefault(c => c.Type == "preferred_username")?.Value ??
                "<anonymous>";
        }
    }
}

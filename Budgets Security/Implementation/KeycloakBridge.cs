using Flurl.Http;
using Keycloak.Net;
using Keycloak.Net.Models.Users;
using Krypton.Budgets.Domain._Ports.Security;
using Microsoft.Extensions.Configuration;

namespace Krypton.Budgets.Security.Implementation;

internal class KeycloakBridge : IUserManagement
{
    private readonly string realm;
    private readonly KeycloakClient client;

    public KeycloakBridge(IConfiguration config)
    {
        realm = config["Security:Admin:Realm"] ?? "";
        client = new KeycloakClient(
            config["Security:Jwt:Authority"],
            config["Security:Admin:Username"],
            config["Security:Admin:Password"],
            config["Security:Admin:AuthRealm"]
        );
    }

    async Task<string> IUserManagement.CreateUserAsync(string email, SecurityLevel level)
    {
        var user = new User
        {
            Enabled = false,
            Email = email
        };

        string userId;
        try
        {
            userId = await client.CreateAndRetrieveUserIdAsync(realm, user);
        }
        catch (FlurlHttpException ex) when (ex.Call.Response.StatusCode == System.Net.HttpStatusCode.Conflict)
        {
            throw new UserManagementException("User email is not unique", ex);
        }

        try
        {
            await ((IUserManagement)this).ChangeSecurityLevelAsync(userId, level);
        }
        catch
        {
            try { await client.DeleteUserAsync(realm, userId); } catch { }
            throw;
        }

        return userId;
    }

    async Task IUserManagement.ChangeUserEmailAsync(string userId, string email)
    {
        var user = await client.GetUserAsync(realm, userId);
        user.Email = email;
        if (!await client.UpdateUserAsync(realm, user.Id, user))
        {
            throw new UserManagementException("Error updating user email");
        }
    }

    async Task IUserManagement.UpdateUserStateAsync(string userId, bool state)
    {
        var user = await client.GetUserAsync(realm, userId);
        user.Enabled = state;
        if (!await client.UpdateUserAsync(realm, user.Id, user))
        {
            throw new UserManagementException("Error updating user state");
        }
    }

    async Task IUserManagement.CancelCredentials(string userId)
    {
        var tasks = (await client.GetUserCredentialsAsync(realm, userId)).
            Select(c => client.DeleteUserCredentialsAsync(realm, userId, c.Id)).
            ToList();

        if ((await Task.WhenAll(tasks)).Any(b => !b))
        {
            throw new UserManagementException("Error cancelling credentials");
        }
    }

    async Task IUserManagement.RequestChangePassword(string userId)
    {
        try
        {
            if (!await client.SendUserUpdateAccountEmailAsync(realm, userId, new[] { "UPDATE_PASSWORD" }))
            {
                throw new UserManagementException("Error requesting password change");
            }
        }
        catch { }
    }

    async Task IUserManagement.ChangeSecurityLevelAsync(string userId, SecurityLevel level)
    {
        var current = await client.GetRealmRoleMappingsForUserAsync(realm, userId);
        if (!await client.DeleteRealmRoleMappingsFromUserAsync(realm, userId, current))
        {
            throw new UserManagementException("Error changing security level");
        }

        try
        {
            var role = await client.GetRoleByNameAsync(realm, level.ToString().ToLower());
            if (!await client.AddRealmRoleMappingsToUserAsync(realm, userId, new[] { role }))
            {
                throw new UserManagementException("Error setting security level for user");
            }
        }
        catch (FlurlHttpException ex) when (ex.Call.Response.StatusCode == System.Net.HttpStatusCode.NotFound) { }
    }

    async Task IUserManagement.DeleteUserAsync(string userId)
    {
        if (!await client.DeleteUserAsync(realm, userId))
        {
            throw new UserManagementException("Error deleting user");
        }
    }
}

namespace Krypton.Budgets.Domain._Ports.Security;

public interface IUserManagement
{
    Task<string> CreateUserAsync(string email, SecurityLevel level);
    Task ChangeUserEmailAsync(string userId, string email);
    Task UpdateUserStateAsync(string userId, bool state);
    Task CancelCredentials(string userId);
    Task RequestChangePassword(string userId);
    Task ChangeSecurityLevelAsync(string userId, SecurityLevel level);
    Task DeleteUserAsync(string userId);
}

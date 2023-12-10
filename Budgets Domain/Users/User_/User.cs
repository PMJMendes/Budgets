using Krypton.Budgets.Domain._Base.Entities;
using Krypton.Budgets.Domain._Impl.Attributes;
using Krypton.Budgets.Domain._Impl.Exceptions;
using Krypton.Budgets.Domain._Impl.Interfaces;
using Krypton.Budgets.Domain._Impl.Utils;
using Krypton.Budgets.Domain._Ports.Security;

namespace Krypton.Budgets.Domain.Users.User_;

internal class User : BaseEntity
{
    private enum UserErrors
    {
        SECURITY_DUPLICATE_EMAIL,
        DISABLED_USER_LOGIN
    }

    protected User() { }

    protected User(IContext context) : base(context) { }

    public User(IContext context, string email, SecurityLevel level, string fullName) : this(context)
    {
        State = UserState.DISABLED;

        Email = email;
        Level = level;
        FullName = fullName;

        create().GetAwaiter().GetResult();

        async Task create()
        {
            try
            {
                ExternalId = await context.UserManagement.CreateUserAsync(email, level);
            }
            catch (UserManagementException ex) when (ex.InnerException is not null)
            {
                throw new UniqueException(TypeHelper.GetProperty((User u) => u.Email),
                    UserErrors.SECURITY_DUPLICATE_EMAIL);
            }
            catch
            {
                try { await context.UserManagement.DeleteUserAsync(ExternalId); } catch { }
                throw;
            }

            await context.Persistence.PersistAsync(this);
        }
    }

    [Required]
    [Unique]
    public virtual string ExternalId { get; protected set; } = "";

    [Required]
    [Unique]
    public virtual string Email { get; protected set; } = "";

    [Required]
    public virtual SecurityLevel Level { get; protected set; }

    [Required]
    public virtual string FullName { get; protected set; } = "";

    [Required]
    public virtual UserState State { get; protected set; } = UserState.DISABLED;

    public virtual async Task ChangeEmailAsync(string email)
    {
        await context.UserManagement.CancelCredentials(ExternalId);

        if (State == UserState.ENABLED)
        {
            State = UserState.PENDING;
        }

        await context.UserManagement.ChangeUserEmailAsync(ExternalId, email);

        if (State == UserState.PENDING)
        {
            await context.UserManagement.RequestChangePassword(ExternalId);
        }

        Email = email;
    }

    public virtual async Task ChangeSecurityLevelAsync(SecurityLevel level)
    {
        await context.UserManagement.ChangeSecurityLevelAsync(ExternalId, level);
        Level = level;
    }

    public virtual void RenameUser(string fullName)
    {
        FullName = fullName;
    }

    public virtual async Task TriggerActivationAsync()
    {
        await context.UserManagement.CancelCredentials(ExternalId);

        if (State == UserState.ENABLED)
        {
            State = UserState.PENDING;
        }

        await context.UserManagement.UpdateUserStateAsync(ExternalId, true);

        if (State == UserState.DISABLED)
        {
            State = UserState.PENDING;
        }

        await context.UserManagement.RequestChangePassword(ExternalId);
    }

    public virtual void UserLoggedIn()
    {
        switch (State)
        {
            case UserState.ENABLED:
                return;

            case UserState.DISABLED:
                throw new ConsistencyException(
                    TypeHelper.GetProperty((User u) => u.State),
                    UserErrors.DISABLED_USER_LOGIN,
                    "Disabled user logged in"
                );

            case UserState.PENDING:
                State = UserState.ENABLED;
                break;
        }
    }

    public virtual async Task DeactivateAsync()
    {
        if (State == UserState.DISABLED)
        {
            return;
        }

        await context.UserManagement.CancelCredentials(ExternalId);

        if (State == UserState.ENABLED)
        {
            State = UserState.PENDING;
        }

        await context.UserManagement.UpdateUserStateAsync(ExternalId, false);

        State = UserState.DISABLED;
    }

    public virtual async Task DeleteAsync()
    {
        await context.UserManagement.DeleteUserAsync(ExternalId);

        await context.Persistence.DeleteAsync(this);
    }
}

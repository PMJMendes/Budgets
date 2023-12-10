using Krypton.Budgets.API._Impl;
using Krypton.Budgets.Domain._Ports.Security;
using Krypton.Budgets.Domain.Users.User_.ChangeEmail;
using Krypton.Budgets.Domain.Users.User_.ChangeSecurityLevel;
using Krypton.Budgets.Domain.Users.User_.RenameUser;

namespace Krypton.Budgets.API.Users.User.UpdateUser;

internal class UpdateUserArgs : TargetArgs, IChangeEmailArgs, IChangeSecurityLevelArgs, IRenameUserArgs
{
    public string? NewEmail { get; set; }

    public SecurityLevel? NewLevel { get; set; }

    public string? NewName { get; set; }
}

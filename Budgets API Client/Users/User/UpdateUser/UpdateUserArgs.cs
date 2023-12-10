using Krypton.Budgets.Blazor.APIClient._Common;

namespace Krypton.Budgets.Blazor.APIClient.Users.User.UpdateUser;

public class UpdateUserArgs : TargetPostArgs
{
    public UpdateUserArgs(Guid targetId, string newEmail, string newLevel, string newName) : base(targetId)
    {
        NewEmail = newEmail;
        NewLevel = newLevel;
        NewName = newName;
    }

    public string NewEmail { get; private init; }

    public string NewLevel { get; private init; }

    public string NewName { get; private init; }
}

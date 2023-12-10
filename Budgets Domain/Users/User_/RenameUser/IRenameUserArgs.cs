using Krypton.Budgets.Domain._Base.Interfaces;
using Krypton.Budgets.Domain._Impl.Attributes;

namespace Krypton.Budgets.Domain.Users.User_.RenameUser;

public interface IRenameUserArgs : ITargetArgs
{
    [Required]
    string? NewName { get; }
}

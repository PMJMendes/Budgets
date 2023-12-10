using Krypton.Budgets.Domain._Base.Interfaces;
using Krypton.Budgets.Domain._Impl.Attributes;
using Krypton.Budgets.Domain._Ports.Security;

namespace Krypton.Budgets.Domain.Global.CreateUser;

public interface ICreateUserArgs : IArguments
{
    [Required]
    string? Email { get; }

    [Required]
    SecurityLevel? Level { get; }

    [Required]
    string? FullName { get; }
}

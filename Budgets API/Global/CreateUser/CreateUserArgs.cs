using Krypton.Budgets.Domain._Ports.Security;
using Krypton.Budgets.Domain.Global.CreateUser;

namespace Krypton.Budgets.API.Global.CreateUser;

internal class CreateUserArgs : ICreateUserArgs
{
    public string? Email { get; set; }

    public SecurityLevel? Level { get; set; }

    public string? FullName { get; set; }
}

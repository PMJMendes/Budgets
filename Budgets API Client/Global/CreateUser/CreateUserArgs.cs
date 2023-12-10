namespace Krypton.Budgets.Blazor.APIClient.Global.CreateUser;

public class CreateUserArgs
{
    public CreateUserArgs(string email, string level, string fullName)
    {
        Email = email;
        Level = level;
        FullName = fullName;
    }

    public string Email { get; private init; }

    public string Level { get; private init; }

    public string FullName { get; private init; }
}

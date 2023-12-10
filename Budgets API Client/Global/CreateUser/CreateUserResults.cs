namespace Krypton.Budgets.Blazor.APIClient.Global.CreateUser;

public class CreateUserResults
{
    public string? Email { get; set; }
    public string? Level { get; set; }
    public string? FullName { get; set; }

    public Guid? NewUserId { get; set; }
    public string? State { get; set; }
}

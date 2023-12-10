namespace Krypton.Budgets.Domain._Ports.Security;

public partial interface ISecurity
{

    bool IsLoggedIn();

    bool HasSecurityLevel(SecurityLevel level);

    string LoginId { get; }

    string LoginTag { get; }
}

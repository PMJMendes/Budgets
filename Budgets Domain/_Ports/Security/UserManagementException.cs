namespace Krypton.Budgets.Domain._Ports.Security;

public class UserManagementException : Exception
{
    public UserManagementException() { }

    public UserManagementException(string message) : base(message) { }

    public UserManagementException(Exception inner) : base(inner.Message, inner) { }

    public UserManagementException(string message, Exception inner) : base(message, inner) { }
}

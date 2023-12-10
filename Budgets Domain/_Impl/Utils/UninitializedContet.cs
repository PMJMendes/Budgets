using Krypton.Budgets.Domain._Impl.Interfaces;
using Krypton.Budgets.Domain._Ports.Hosting;
using Krypton.Budgets.Domain._Ports.Persistence;
using Krypton.Budgets.Domain._Ports.Security;
using Krypton.Budgets.Domain.Global;
using Krypton.Budgets.Domain.Users.User_;

namespace Krypton.Budgets.Domain._Impl.Utils;

internal class UninitializedContet : IContext
{
    public static readonly UninitializedContet Instance = new();

    private UninitializedContet() { }

    public GlobalObject GlobalObject => throw new NotImplementedException();
    public IPersistence Persistence => throw new NotImplementedException();
    public ISecurity Security => throw new NotImplementedException();
    public IHost DomainClient => throw new NotImplementedException();
    public IUserManagement UserManagement => throw new NotImplementedException();
    public User? CurrentUser => throw new NotImplementedException();
}

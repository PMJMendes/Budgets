using Krypton.Budgets.Domain._Ports.Hosting;
using Krypton.Budgets.Domain._Ports.Persistence;
using Krypton.Budgets.Domain._Ports.Security;
using Krypton.Budgets.Domain.Global;
using Krypton.Budgets.Domain.Users.User_;

namespace Krypton.Budgets.Domain._Impl.Interfaces;

internal interface IContext
{
    GlobalObject GlobalObject { get; }

    IPersistence Persistence { get; }

    ISecurity Security { get; }

    IHost DomainClient { get; }

    IUserManagement UserManagement { get; }

    User? CurrentUser { get; }
}

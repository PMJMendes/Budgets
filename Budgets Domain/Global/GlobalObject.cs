using Krypton.Budgets.Domain._Impl.Interfaces;
using Krypton.Budgets.Domain._Ports.Hosting;
using Krypton.Budgets.Domain._Ports.Persistence;
using Krypton.Budgets.Domain._Ports.Security;
using Krypton.Budgets.Domain.Budgets.Budget_;
using Krypton.Budgets.Domain.Users.User_;

namespace Krypton.Budgets.Domain.Global
{
    internal class GlobalObject : IContext
    {
        private User? current;

        public GlobalObject(IPersistence persistence, ISecurity security, IHost domainClient, IUserManagement userManagement)
        {
            Persistence = persistence;
            Security = security;
            DomainClient = domainClient;
            UserManagement = userManagement;
        }

        GlobalObject IContext.GlobalObject => this;
        public IPersistence Persistence { get; private init; }
        public ISecurity Security { get; private init; }
        public IHost DomainClient { get; private init; }
        public IUserManagement UserManagement { get; private init; }
        public User? CurrentUser => current?.ExternalId == Security.LoginId ? current : current = GetUserFromSecureId();

        public async Task ChangeCurrentUserPasswordAsync()
        {
            var user = CurrentUser;

            if (user is not null)
            {
                await user.TriggerActivationAsync();
            }
        }

        public User CreateUser(string email, SecurityLevel level, string fullName)
        {
            return new User(this, email, level, fullName);
        }

        public Budget CreateBudget(string code, bool asTemplate, DateOnly budgetDate, string title)
        {
            return new Budget(this, code, asTemplate, budgetDate, title);
        }

        private User? GetUserFromSecureId()
        {
            return Persistence.Query<User>().Where(u => u.ExternalId == Security.LoginId).SingleOrDefault();
        }
    }
}

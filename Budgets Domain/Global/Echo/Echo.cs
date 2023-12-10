using Krypton.Budgets.Domain._Base.Interfaces;
using Krypton.Budgets.Domain._Base.Services;
using Krypton.Budgets.Domain._Impl.Interfaces;
using Krypton.Budgets.Domain._Impl.Utils;
using Krypton.Budgets.Domain._Ports.Security;
using Microsoft.Extensions.Logging;

namespace Krypton.Budgets.Domain.Global.Echo
{
    internal class Echo : BaseQuery<Echo, IArguments, IEchoItem>, IEcho
    {
        public Echo(IContext context, ILogger<Echo> logger) : base(context, logger) { }

        protected override void AssertPermissions() => AssertIsLoggedIn();

        protected override IAsyncEnumerable<V> InnerFetch<V>(IArguments? _, Func<IEchoItem, V> itemFactory)
        {
            var user = context.CurrentUser;

            return itemFactory(new Item(
                user?.FullName ?? context.Security.LoginTag,
                user?.Level
            )).AsSingleEnumerator().ToAsyncEnumerable();
        }

        private record struct Item(
            string Name,
            SecurityLevel? Level
        ) : IEchoItem;
    }
}

using Krypton.Budgets.Domain._Base.Interfaces;
using Krypton.Budgets.Tests.API._Impl;
using Krypton.Budgets.Tests.Utils;
using Moq;
using System.Reflection;

namespace Krypton.Budgets.Tests.Domain._Impl
{
    internal class QueryResultsTestFixture<T, A, R> : TestFixtureBase
        where A : class, IArguments
        where R : class, IQueryResultItem
        where T : class, IQuery<A, R>
    {
        private static readonly MethodInfo _setupMethod = typeof(QueryResultsTestFixture<T, A, R>).GetMethod(nameof(QueryResultsTestFixture<T, A, R>.SetupDomainCall), BindingFlags.NonPublic | BindingFlags.Instance)!;
        private static readonly MethodInfo _setupExMethod = typeof(QueryResultsTestFixture<T, A, R>).GetMethod(nameof(QueryResultsTestFixture<T, A, R>.SetupExceptionCall), BindingFlags.NonPublic | BindingFlags.Instance)!;

        private readonly Mock<T> op;

        public QueryResultsTestFixture(Mock<T> op, Type type)
        {
            this.op = op;

            SetupMethod = _setupMethod.MakeGenericMethod(type);
            SetupExMethod = _setupExMethod.MakeGenericMethod(type);
        }

        public MethodInfo SetupMethod { get; internal init; }

        public MethodInfo SetupExMethod { get; internal init; }

        private void SetupDomainCall<V>(A? args, IEnumerable<R> results) where V : R
        {
            op.Setup(p => p.Fetch(It.IsAny<A>(), It.IsAny<Func<R, V>>())).
                Callback((A actualArgs, Func<R, V> _) => LocalAssert.AreEquivalent(args, actualArgs)).
                Returns((A _, Func<R, V> func) => results.Select(r => func(r)).ToAsyncEnumerable());
        }

        private void SetupExceptionCall<V>(A? args, Exception ex) where V : R
        {
            op.Setup(p => p.Fetch(It.IsAny<A>(), It.IsAny<Func<R, V>>())).
                Callback((A actualArgs, Func<R, V> _) => LocalAssert.AreEquivalent(args, actualArgs)).
                Throws(ex);
        }
    }
}

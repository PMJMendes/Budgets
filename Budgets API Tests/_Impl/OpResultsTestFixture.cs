using Krypton.Budgets.Domain._Base.Interfaces;
using Krypton.Budgets.Tests.API._Impl;
using Krypton.Budgets.Tests.Utils;
using Moq;
using System.Reflection;

namespace Krypton.Budgets.Tests.Domain._Impl
{
    internal class OpResultsTestFixture<T, A, R> : TestFixtureBase
        where A : class, IArguments
        where R : class, IOpResults
        where T : class, IOperation<A, R>
    {
        private static readonly MethodInfo _setupMethod = typeof(OpResultsTestFixture<T, A, R>).GetMethod(nameof(OpResultsTestFixture<T, A, R>.SetupDomainCall), BindingFlags.NonPublic | BindingFlags.Instance)!;
        private static readonly MethodInfo _setupExMethod = typeof(OpResultsTestFixture<T, A, R>).GetMethod(nameof(OpResultsTestFixture<T, A, R>.SetupExceptionCall), BindingFlags.NonPublic | BindingFlags.Instance)!;

        private readonly Mock<T> op;

        public OpResultsTestFixture(Mock<T> op, Type type)
        {
            this.op = op;

            SetupMethod = _setupMethod.MakeGenericMethod(type);
            SetupExMethod = _setupExMethod.MakeGenericMethod(type);
        }

        public MethodInfo SetupMethod { get; internal init; }

        public MethodInfo SetupExMethod { get; internal init; }

        private void SetupDomainCall<V>(A? args, R results) where V : R
        {
            op.Setup(p => p.Execute(It.IsAny<A>(), It.IsAny<Func<R, V>>())).
                Callback((A actualArgs, Func<R, V> _) => LocalAssert.AreEquivalent(args, actualArgs)).
                Returns((A _, Func<R, V> func) => Task.FromResult(func(results)));
        }

        private void SetupExceptionCall<V>(A? args, Exception ex) where V : class, R
        {
            op.Setup(p => p.Execute(It.IsAny<A>(), It.IsAny<Func<R, V>>())).
                Callback((A actualArgs, Func<R, V> _) => LocalAssert.AreEquivalent(args, actualArgs)).
                ThrowsAsync(ex);
        }
    }
}

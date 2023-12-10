using Krypton.Budgets.Domain._Base.Interfaces;
using Krypton.Budgets.Domain._Ports.Persistence;
using Krypton.Budgets.Persistence.Hosting;
using Krypton.Budgets.Tests.API._Impl;
using Krypton.Budgets.Tests.Domain._Impl;
using Krypton.Budgets.Tests.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System.Reflection;

namespace Krypton.Budgets.Tests.API._Base;

public class TestBase : CommonTestBase, IAsyncLifetime
{
    protected static readonly string TOP_LEVEL_NAMESPACE = "Krypton.Budgets";
    protected static readonly string API_NAMESPACE = TOP_LEVEL_NAMESPACE + ".API";
    protected static readonly string DOMAIN_NAMESPACE = TOP_LEVEL_NAMESPACE + ".Domain";
    protected static readonly Assembly DOMAIN_ASSEMBLY = typeof(IEntity).Assembly;
    protected static readonly Assembly API_ASSEMBLY = typeof(Program).Assembly;

    protected readonly Mock<ILogger> logger;

    protected readonly HttpClient client;

    private readonly Dictionary<Type, Mock> domainMocks;
    private readonly WebApplicationFactory<Program> app;
    private readonly Dictionary<Type, TestFixtureBase> fixtures = new();
    private readonly Mock<IPersistenceHosting> migrator = new(MockBehavior.Strict);

    public TestBase(IEnumerable<Mock> domainMocks)
    {
        this.domainMocks = domainMocks.ToDictionary(m => m.GetType().GetGenericArguments()[0], m => m);

        logger = new();

        app = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.
                ConfigureLogging(logging => logging.ClearProviders()).
                ConfigureServices(services =>
                {
                    var testedServices = services.
                        Where(s => (s.ServiceType.Namespace ?? "").StartsWith(API_NAMESPACE) &&
                            s.ImplementationType?.Name == GetType().Name.Replace("Test", "")).
                        ToList();

                    var localServices = services.
                        Where(s => ((s.ServiceType.Namespace ?? "").StartsWith(TOP_LEVEL_NAMESPACE) ||
                            (s.ImplementationType?.Namespace ?? "").StartsWith(TOP_LEVEL_NAMESPACE) ||
                            (s.ImplementationInstance?.GetType().Namespace ?? "").StartsWith(TOP_LEVEL_NAMESPACE) ||
                            (s.ImplementationFactory?.GetType().GetGenericArguments()[1].Namespace ?? "").StartsWith(TOP_LEVEL_NAMESPACE)) &&
                            !testedServices.Contains(s) &&
                            s.ServiceType.Name != nameof(IDomainExplorer)).
                        ToList();

                    foreach (var s in localServices)
                    {
                        services.Remove(s);
                    }

                    foreach (var mock in domainMocks)
                    {
                        services.AddSingleton(mock.GetType().GetGenericArguments()[0], mock.Object);
                    }

                    services.AddSingleton(logger.Object).
                        AddSingleton(typeof(ILogger<>), typeof(ProxyLogger<>)).
                        AddSingleton(migrator.Object).
                        BuildServiceProvider();
                });
        });

        migrator.Setup(m => m.RunMigrations());

        client = app.CreateClient();

        migrator.VerifyAll();
    }

    public Task InitializeAsync() => Task.CompletedTask;
    public async Task DisposeAsync()
    {
        client.Dispose();
        await app.DisposeAsync();
    }

    protected Mock<T> GetMock<T>() where T : class => (Mock<T>)domainMocks[typeof(T)];

    protected void SetupOpCall<T, A, R>(Mock<T> mock, A args, R results)
        where A : class, IArguments
        where R : class, IOpResults
        where T : class, IOperation<A, R>
    {
        var fixture = (OpResultsTestFixture<T, A, R>)FindOpFixture<T, A, R>(mock);
        fixture.SetupMethod.Invoke(fixture, new object[] { args, results });
    }

    protected void SetupQueryCall<T, A, R>(Mock<T> mock, A args, IEnumerable<R> results)
        where A : class, IArguments
        where R : class, IQueryResultItem
        where T : class, IQuery<A, R>
    {
        var fixture = (QueryResultsTestFixture<T, A, R>)FindQueryFixture<T, A, R>(mock);
        fixture.SetupMethod.Invoke(fixture, new object[] { args, results });
    }

    protected void SetupOpExCall<T, A, R>(Mock<T> mock, A args, R? _, Exception ex)
        where A : class, IArguments
        where R : class, IOpResults
        where T : class, IOperation<A, R>
    {
        var fixture = (OpResultsTestFixture<T, A, R>)FindOpFixture<T, A, R>(mock);
        fixture.SetupExMethod.Invoke(fixture, new object[] { args, ex });
    }

    protected void SetupQueryExCall<T, A, R>(Mock<T> mock, A args, R? _, Exception ex)
        where A : class, IArguments
        where R : class, IQueryResultItem
        where T : class, IQuery<A, R>
    {
        var fixture = (QueryResultsTestFixture<T, A, R>)FindQueryFixture<T, A, R>(mock);
        fixture.SetupExMethod.Invoke(fixture, new object[] { args, ex });
    }

    private TestFixtureBase FindOpFixture<T, A, R>(Mock<T> mock)
        where A : class, IArguments
        where R : class, IOpResults
        where T : class, IOperation<A, R>
    {
        return fixtures.GetValueOrDefault(typeof(R)) ?? (fixtures[typeof(R)] = BuildOpFixture<T, A, R>(mock));
    }

    private TestFixtureBase FindQueryFixture<T, A, R>(Mock<T> mock)
        where A : class, IArguments
        where R : class, IQueryResultItem
        where T : class, IQuery<A, R>
    {
        return fixtures.GetValueOrDefault(typeof(R)) ?? (fixtures[typeof(R)] = BuildQueryFixture<T, A, R>(mock));
    }

    private static TestFixtureBase BuildOpFixture<T, A, R>(Mock<T> mock)
        where A : class, IArguments
        where R : class, IOpResults
        where T : class, IOperation<A, R>
    {
        var resultsType = FindType(API_ASSEMBLY, API_NAMESPACE, typeof(R));
        return new OpResultsTestFixture<T, A, R>(mock, resultsType);
    }

    private static TestFixtureBase BuildQueryFixture<T, A, R>(Mock<T> mock)
        where A : class, IArguments
        where R : class, IQueryResultItem
        where T : class, IQuery<A, R>
    {
        var resultsType = FindType(API_ASSEMBLY, API_NAMESPACE, typeof(R));
        return new QueryResultsTestFixture<T, A, R>(mock, resultsType);
    }
}

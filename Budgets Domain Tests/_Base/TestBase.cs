using Krypton.Budgets.Domain._Base.Interfaces;
using Krypton.Budgets.Domain._Ports.Hosting;
using Krypton.Budgets.Domain._Ports.Persistence;
using Krypton.Budgets.Domain._Ports.Security;
using Krypton.Budgets.Tests.Domain._Impl;
using Krypton.Budgets.Tests.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Krypton.Budgets.Tests.Domain._Base;

public class TestBase<T> : CommonTestBase, IDisposable where T : class
{
    protected static readonly BindingFlags _FLAGS = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
    protected static readonly string DOMAIN_NAMESPACE = "Krypton.Budgets.Domain";
    protected static readonly Assembly _ASSEMBLY = typeof(IEntity).Assembly;

    protected readonly ServiceProvider sp;

    protected readonly Mock<IPersistence> persistence;
    protected readonly Mock<ISecurity> security;
    protected readonly Mock<IUserManagement> userManagement;
    protected readonly Mock<IHost> host;
    protected readonly Mock<ILogger> logger;

    protected readonly T subject;

    private readonly Dictionary<Type, EntityTestFixture> fixtures = new();
    private readonly Dictionary<Guid, IEntity> entities = new();

    public TestBase()
    {
        persistence = new Mock<IPersistence>(MockBehavior.Strict);
        security = new Mock<ISecurity>(MockBehavior.Strict);
        userManagement = new Mock<IUserManagement>(MockBehavior.Strict);
        host = new Mock<IHost>(MockBehavior.Strict);
        logger = new Mock<ILogger>();

        var builder = new ServiceCollection().
            AddBudgetsDomain().
            AddSingleton(persistence.Object).
            AddSingleton(security.Object).
            AddSingleton(userManagement.Object).
            AddSingleton(host.Object).
            AddSingleton(logger.Object).
            AddSingleton(typeof(ILogger<>), typeof(ProxyLogger<>));
        sp = builder.BuildServiceProvider();

        subject = sp.GetRequiredService<T>();

        host.Setup(h => h.CorrelationId).Returns(Guid.NewGuid().ToString());
    }

    void IDisposable.Dispose()
    {
        sp.Dispose();
        GC.SuppressFinalize(this);
    }

    protected void SetupUser(bool isAdmin, Guid? userId = null, string? externId = null, string? tag = null)
    {
        security.Setup(s => s.IsLoggedIn()).Returns(true);
        security.Setup(s => s.HasSecurityLevel(SecurityLevel.Admin)).Returns(isAdmin);
        security.Setup(s => s.HasSecurityLevel(SecurityLevel.Assistant)).Returns(true);
        security.Setup(s => s.LoginTag).Returns(tag ?? (isAdmin ? "admin" : "user"));
        security.Setup(s => s.LoginId).Returns(externId ?? "?");

        if (userId is Guid realId)
        {
            SetupTestData("User", new[]
            {
                new
                {
                    Id = realId,
                    ExternalId = externId ?? "?"
                }
            });
        }
    }

    protected object[] SetupTestData(string entityName, object[] data)
    {
        var fixture = FindFixture(entityName);
        fixture.TestData = fixture.TestData.Concat(data.Select(d => BuildEntity(fixture.EntityType, d)).ToList().AsQueryable()).Distinct();

        fixture.SetupMethod.Invoke(fixture, Array.Empty<object>());

        return data;
    }

    protected object[] ResetTestData(string entityName, object[] data)
    {
        var fixture = FindFixture(entityName);
        fixture.TestData = data.Select(d => BuildEntity(fixture.EntityType, d)).ToList().AsQueryable();

        return data;
    }

    protected void SetEntityId(IEntity instance, Guid id)
    {
        instance.GetType().GetProperty("Id", _FLAGS)?.SetValue(instance, id);
    }

    private EntityTestFixture FindFixture(string entityName)
    {
        var type = FindType(_ASSEMBLY, DOMAIN_NAMESPACE, entityName);
        return fixtures.GetValueOrDefault(type) ?? (fixtures[type] = new EntityTestFixture(persistence, type));
    }

    private IEntity BuildEntity(Type type, object? data)
    {
        Guid? newId = null;
        if (data?.GetType().GetProperty("Id")?.GetValue(data) is Guid dataId)
        {
            if (entities.TryGetValue(dataId, out IEntity? cached))
            {
                return cached;
            }
            newId = dataId;
        }

        var constructors = type.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Where(c => c.GetParameters().Any());
        foreach (var c in constructors)
        {
            var allParams = new Dictionary<string, ParameterInfo>(c.GetParameters().Select(p => KeyValuePair.Create(p.Name ?? "", p)));
            var serviceValues = new Dictionary<string, object>(allParams.
                Select(pkv => KeyValuePair.Create(pkv.Key, sp.GetService(pkv.Value.ParameterType)!)).
                Where(kv => kv.Value is not null));
            Dictionary<string, object?> dataValues = (data is null) ? new() : new(data.GetType().GetProperties(_FLAGS).
                Select(pr => KeyValuePair.Create(pr.Name, pr.GetValue(data))));

            if (allParams.All(kv => serviceValues.ContainsKey(kv.Key)) && serviceValues.All(kv => allParams.ContainsKey(kv.Key)))
            {
                var ordered = allParams.Select(pkv => serviceValues[pkv.Key]).ToArray();

                IEntity instance = (IEntity)c.Invoke(ordered);
                if (instance.GetType().GetProperty("Id") is PropertyInfo idProp && newId is Guid id)
                {
                    entities[id] = instance;
                }

                foreach (var kv in dataValues)
                {
                    var target = TestBase<T>.GetMemberInfo(instance, kv.Key);
                    if (target == null)
                    {
                        Assert.Fail($"Unable to set property {kv.Key} on entity type {instance.GetType().Name}");
                    }

                    var propType = (target as PropertyInfo)?.PropertyType ?? (target as FieldInfo)?.FieldType ?? typeof(object);
                    var value = TranslateValue(kv.Value, propType);

                    (target as PropertyInfo)?.SetValue(instance, value);
                    (target as FieldInfo)?.SetValue(instance, value);
                }

                return instance;
            }
        }

        Assert.Fail("Unable to find constructor for entity");
        return null;
    }

    private static MemberInfo GetMemberInfo(IEntity instance, string name)
    {
        return instance.GetType().GetProperty(name, _FLAGS) as MemberInfo ?? instance.GetType().GetField(name, _FLAGS) ??
            throw new Exception("Property or field {name} not found");
    }

    private object? TranslateValue(object? value, Type propType)
    {
        if (value is IEnumerable<object> enumerable && typeof(IEnumerable).IsAssignableFrom(propType))
        {
            var elemType = propType.IsGenericType ? propType.GetGenericArguments()[0] : typeof(object);
            var translator = fixtures.GetValueOrDefault(elemType) ?? new TypeTranslator(elemType);

            return translator.ToHashSetMethod.Invoke(null, new[]
            {
                translator.CastMethod.Invoke(null, new[]
                {
                    enumerable.Select(d => BuildEntity(elemType, d)).ToList()
                })
            });
        }

        if ((value?.GetType().GetCustomAttributes(typeof(CompilerGeneratedAttribute), false).Length ?? 0) > 0)
        {
            return BuildEntity(propType, value);
        }

        return value;
    }
}

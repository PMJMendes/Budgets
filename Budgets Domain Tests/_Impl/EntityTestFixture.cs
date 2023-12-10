using Krypton.Budgets.Domain._Base.Interfaces;
using Krypton.Budgets.Domain._Ports.Persistence;
using Moq;
using System.Reflection;

namespace Krypton.Budgets.Tests.Domain._Impl;

internal class EntityTestFixture : TypeTranslator
{
    private static readonly MethodInfo _setupMethod = typeof(EntityTestFixture).GetMethod(nameof(EntityTestFixture.SetupQueryCall), BindingFlags.NonPublic | BindingFlags.Instance)!;

    private readonly Mock<IPersistence> persistence;

    public EntityTestFixture(Mock<IPersistence> persistence, Type type) : base(type)
    {
        this.persistence = persistence;
        EntityType = type;

        SetupMethod = _setupMethod.MakeGenericMethod(type);

        TestData = Array.Empty<IEntity>().AsQueryable();
    }

    public Type EntityType { get; internal init; }

    public MethodInfo SetupMethod { get; internal init; }

    public IQueryable<IEntity> TestData { get; set; }

    private void SetupQueryCall<T>() where T : IEntity
    {
        persistence.Setup(p => p.Query<T>()).Returns(() => TestData.Cast<T>());
    }

    private void VerifyCall<T>() where T : IEntity
    {
        persistence.Verify(p => p.Query<T>());
    }
}

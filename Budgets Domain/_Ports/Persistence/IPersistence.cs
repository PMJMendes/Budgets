using Krypton.Budgets.Domain._Base.Interfaces;

namespace Krypton.Budgets.Domain._Ports.Persistence;

public interface IPersistence
{
    Task<T> GetAsync<T>(Guid id) where T : IEntity;

    IQueryable<T> Query<T>() where T : IEntity;

    Task PersistAsync<T>(T obj) where T : IEntity;
    Task DeleteAsync<T>(T t) where T : IEntity;

    Task FlushChangesAsync();
}

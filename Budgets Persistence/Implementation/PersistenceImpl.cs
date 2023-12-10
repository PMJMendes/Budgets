using Krypton.Budgets.Domain._Ports.Persistence;
using NHibernate;
using System.Data;

namespace Krypton.Budgets.Persistence.Implementation;

internal class PersistenceImpl : IPersistence, IDisposable
{
    private ISession? session;
    private ITransaction? current;

    public PersistenceImpl(ISession session)
    {
        this.session = session;
        current = session.BeginTransaction(IsolationLevel.Serializable);
    }

    Task<T> IPersistence.GetAsync<T>(Guid id)
    {
        return (session ?? throw new Exception("Inconsistent persistence state")).GetAsync<T>(id);
    }

    IQueryable<T> IPersistence.Query<T>()
    {
        return session?.Query<T>() ?? throw new Exception("Inconsistent persistence state");
    }

    async Task IPersistence.PersistAsync<T>(T obj)
    {
        try
        {
            await (session?.PersistAsync(obj) ?? throw new Exception("Inconsistent persistence state"));
        }
        catch
        {
            InvalidateSession();
            throw;
        }
    }

    async Task IPersistence.DeleteAsync<T>(T obj)
    {
        try
        {
            await (session?.DeleteAsync(obj) ?? throw new Exception("Inconsistent persistence state"));
        }
        catch
        {
            InvalidateSession();
            throw;
        }
    }

    async Task IPersistence.FlushChangesAsync()
    {
        try
        {
            await (session?.FlushAsync() ?? throw new Exception("Inconsistent persistence state"));
        }
        catch
        {
            InvalidateSession();
            throw;
        }
    }

    void IDisposable.Dispose()
    {
        current?.Commit();
        session?.Close();
        GC.SuppressFinalize(this);
    }

    private void InvalidateSession()
    {
        current?.Rollback();
        current = null;
        session = null;
    }
}

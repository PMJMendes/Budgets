namespace Krypton.Budgets.Domain._Base.Interfaces;

public interface IQuery<T, U> where T : IArguments where U : IQueryResultItem
{
    IAsyncEnumerable<V> Fetch<V>(T? args, Func<U, V> itemFactory) where V : U;
}

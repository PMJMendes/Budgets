namespace Krypton.Budgets.Domain._Base.Interfaces;

public interface IOperation<T, U> where T : IArguments where U : IOpResults
{
    Task<V> Execute<V>(T? args, Func<U, V> results) where V : U;
}

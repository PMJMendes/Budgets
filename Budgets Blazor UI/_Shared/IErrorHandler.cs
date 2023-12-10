using Krypton.Budgets.Blazor.APIClient._Common;

namespace Krypton.Budgets.Blazor.UI._Shared;

public interface IErrorHandler
{
    Task CheckErrorsAsync<T>(SafeResult<T> result, Action<T> handleResult);

    Task CheckErrorsAsync<T>(SafeResult<T> result, Func<T, Task> handleResult);
}

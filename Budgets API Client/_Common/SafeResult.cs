using Krypton.Budgets.Blazor.APIClient._Impl;

namespace Krypton.Budgets.Blazor.APIClient._Common;

public class SafeResult<R>
{
    private readonly R? result;
    private readonly ErrorResults? errors;

    public SafeResult(R? result)
    {
        R notNull = result ?? throw
            new ServiceErrorException(ServiceErrorException.ErrorType.INIT_NULL_RESULT);
        this.result = notNull;

        errors = default;
    }

    public SafeResult(ErrorResults? errors)
    {
        ErrorResults notNull = errors ?? throw
            new ServiceErrorException(ServiceErrorException.ErrorType.INIT_NULL_RESULT);
        this.errors = notNull;

        result = default;
    }

    public bool IsResult => result is not null;

    public bool IsErrors => errors is not null;

    public R Result =>
        result ?? throw new ServiceErrorException(ServiceErrorException.ErrorType.NOT_A_RESULT);

    public ErrorResults Errors =>
        errors ?? throw new ServiceErrorException(ServiceErrorException.ErrorType.NOT_AN_ERROR);
}

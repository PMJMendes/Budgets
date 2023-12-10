namespace Krypton.Budgets.Blazor.APIClient._Impl;

public class ServiceErrorException : Exception
{
    public enum ErrorType
    {
        INIT_NULL_RESULT,
        INIT_NULL_ERROR,
        NOT_A_RESULT,
        NOT_AN_ERROR
    }

    public ServiceErrorException(ErrorType type)
    {
        Type = type;
    }

    public ErrorType Type { get; private init; }
}

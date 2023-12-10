namespace Krypton.Budgets.API._Impl;

internal readonly struct ErrorList
{
    public ErrorList(int status, IEnumerable<Error> errors)
    {
        Status = status;
        InvalidParams = errors;
    }

#pragma warning disable CA1822 // Mark members as static
    public string Type => "http://app/validation-error";
#pragma warning restore CA1822 // Mark members as static

    public int Status { get; private init; }

    public IEnumerable<Error> InvalidParams { get; private init; }
}

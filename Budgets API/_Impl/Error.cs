namespace Krypton.Budgets.API._Impl;

internal readonly struct Error
{
    public Error(string name, string reason, string message, Dictionary<string, object>? attributes = null)
    {
        Name = name;
        Reason = reason;
        Message = message;
        Attributes = attributes ?? new();
    }

    public string Name { get; }
    public string Reason { get; }
    public string Message { get; }
    public Dictionary<string, object> Attributes { get; }
}

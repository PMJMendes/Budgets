using Krypton.Budgets.Blazor.APIClient._Base;

namespace Krypton.Budgets.Blazor.APIClient._Impl;

public class EmptyQueryArgs : IQueryArgs
{
    public static readonly EmptyQueryArgs Instance = new();

    private EmptyQueryArgs() { }

    public Dictionary<string, string?> GetQueryString() => new();
}

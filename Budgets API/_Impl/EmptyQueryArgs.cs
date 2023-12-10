using Krypton.Budgets.Domain._Base.Interfaces;

namespace Krypton.Budgets.API._Impl;

internal readonly struct EmptyQueryArgs : IArguments
{
    public static readonly EmptyQueryArgs Instance = new();
}

using Krypton.Budgets.Domain._Base.Interfaces;

namespace Krypton.Budgets.API._Impl;

internal readonly struct EmptyArgs : IArguments
{
    public static readonly EmptyArgs Instance = new();
}

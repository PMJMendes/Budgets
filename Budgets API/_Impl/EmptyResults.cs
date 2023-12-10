using Krypton.Budgets.Domain._Base.Interfaces;

namespace Krypton.Budgets.API._Impl;

internal readonly struct EmptyResults : IOpResults
{
    public static readonly EmptyResults Instance = new();
}

using Krypton.Budgets.Domain._Base.Interfaces;

namespace Krypton.Budgets.JobRunner.Implementation;

internal class EmptyArgs : IArguments
{
    public static EmptyArgs Instance = new();

    private EmptyArgs() { }
}

using Krypton.Budgets.Domain._Base.Interfaces;

namespace Krypton.Budgets.API._Impl;

internal class TargetArgs : ITargetArgs
{
    public Guid? TargetId { get; set; }
}

using Krypton.Budgets.Domain._Base.Interfaces;

namespace Krypton.Budgets.API._Impl;

internal struct TargetQueryArgs : ITargetArgs
{
    public Guid? TargetId { get; set; }
}

using Krypton.Budgets.Domain._Impl.Attributes;

namespace Krypton.Budgets.Domain._Base.Interfaces;

public interface ITargetArgs : IArguments
{
    [Required]
    Guid? TargetId { get; }
}

using Krypton.Budgets.Domain._Base.Interfaces;

namespace Krypton.Budgets.Domain.Budgets.Budget_.AssignManager;

public interface IAssignManagerArgs : ITargetArgs
{
    Guid? NewManagerId { get; set; }
}

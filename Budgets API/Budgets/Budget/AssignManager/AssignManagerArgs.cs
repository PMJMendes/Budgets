using Krypton.Budgets.API._Impl;
using Krypton.Budgets.Domain.Budgets.Budget_.AssignManager;

namespace Krypton.Budgets.API.Budgets.Budget.AssignManager;

internal class AssignManagerArgs : TargetArgs, IAssignManagerArgs
{
    public Guid? NewManagerId { get; set; }
}

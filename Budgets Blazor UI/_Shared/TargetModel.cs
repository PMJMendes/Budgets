using Krypton.Budgets.Blazor.APIClient._Common;

namespace Krypton.Budgets.Blazor.UI._Shared;

public class TargetModel
{
    private TargetModel(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; private init; }

    public TargetQueryArgs AsQueryArgs => new(Id);
    public TargetPostArgs AsPostArgs => new(Id);

    public static TargetModel Empty => new(Guid.Empty);

    public static TargetModel ForId(Guid id) => new(id);
}

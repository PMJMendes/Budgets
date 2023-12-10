using Krypton.Budgets.Domain._Base.Entities;
using Krypton.Budgets.Domain._Impl.Attributes;
using Krypton.Budgets.Domain._Impl.Interfaces;
using Krypton.Budgets.Domain.Budgets.Item_;
using Krypton.Budgets.Shared;

namespace Krypton.Budgets.Domain.Budgets.Cost_;

internal class Cost : BaseEntity, ICost
{
    protected Cost() { }

    protected Cost(IContext context) : base(context) { }

    public Cost(IContext context, Item owner, int order, CostData data) : this(context)
    {
        Owner = owner;
        Order = order;

        UpdateData(data);

        context.Persistence.PersistAsync(this).GetAwaiter().GetResult();
    }

    [Required]
    [Unique(Combinations = "businessKey")]
    public virtual Item Owner { get; protected set; } = default!;

    [Required]
    [Unique(Combinations = "businessKey")]
    public virtual int Order { get; protected set; } = 0;

    [Required]
    public virtual decimal CostValue { get; protected set; } = 0m;

    public string? Supplier { get; protected set; }
    public string? Text { get; protected set; }

    public void ReRank(int i) => Order = i;

    public virtual void UpdateData(CostData data)
    {
        CostValue = data.CostValue;
        Supplier = data.Supplier;
        Text = data.Text;
    }

    public virtual async Task DeleteAsync()
    {
        Owner.RemoveCost(this);

        await context.Persistence.DeleteAsync(this);
    }
}

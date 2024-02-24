using Krypton.Budgets.Domain._Base.Entities;
using Krypton.Budgets.Domain._Impl.Attributes;
using Krypton.Budgets.Domain._Impl.Interfaces;
using Krypton.Budgets.Domain.Budgets.Category_;
using Krypton.Budgets.Shared;
using ValueType = Krypton.Budgets.Shared.ValueType;

namespace Krypton.Budgets.Domain.Budgets.ValueDef_;

internal class ValueDef : BaseEntity, IValueDef
{
    protected ValueDef() { }

    protected ValueDef(IContext context) : base(context) { }

    public ValueDef(IContext context, Category owner, int order, ValueDefData def) : this(context)
    {
        Owner = owner;
        Order = order;

        UpdateDefinition(def);

        context.Persistence.PersistAsync(this).GetAwaiter().GetResult();
    }

    [Required]
    public virtual Category Owner { get; protected set; } = default!;

    [Required]
    public virtual int Order { get; protected set; } = 0;

    [Required]
    public virtual ValueType Type { get; protected set; }

    public virtual string? Description { get; protected set; } = "";

    public virtual string? DescEnglish { get; protected set; }

    public virtual string? BCAFormula { get; protected set; }

    internal void ReRank(int i) => Order = i;

    public async Task<ValueDef> CloneAsync(Category newOwner)
    {
        var result = new ValueDef(context)
        {
            Owner = newOwner,

            Order = Order,
            Type = Type,
            Description = Description,
            DescEnglish = DescEnglish,
            BCAFormula = BCAFormula
        };

        await context.Persistence.PersistAsync(result);

        return result;
    }

    public virtual void UpdateDefinition(ValueDefData data)
    {
        Type = data.Type;
        Description = data.Description;
        DescEnglish = data.DescEnglish;
        BCAFormula = data.BCAFormula;
    }

    public virtual async Task DeleteAsync()
    {
        await Owner.DeleteValuesAndRemoveValueDefAsync(this);

        await context.Persistence.DeleteAsync(this);
    }
}

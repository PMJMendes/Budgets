using Krypton.Budgets.Domain._Base.Entities;
using Krypton.Budgets.Domain._Impl.Attributes;
using Krypton.Budgets.Domain._Impl.Interfaces;
using Krypton.Budgets.Domain.Budgets.Item_;
using Krypton.Budgets.Domain.Budgets.ValueDef_;
using Krypton.Budgets.Shared;
using ValueType = Krypton.Budgets.Shared.ValueType;

namespace Krypton.Budgets.Domain.Budgets.Value_;

internal class Value : BaseEntity, IValue
{
    protected Value() { }

    protected Value(IContext context) : base(context) { }

    public Value(IContext context, Item owner, ValueDef def, ValueData data) : this(context)
    {
        Owner = owner;
        Def = def;

        UpdateData(data);

        context.Persistence.PersistAsync(this).GetAwaiter().GetResult();
    }

    [Required]
    [Unique(Combinations = "businessKey")]
    public virtual Item Owner { get; protected set; } = default!;
    [External]
    IItem IValue.Owner => Owner;

    [Required]
    [Unique(Combinations = "businessKey")]
    public virtual ValueDef Def { get; protected set; } = default!;
    [External]
    IValueDef IValue.Def => Def;

    public virtual decimal? NumberValue { get; protected set; }

    public virtual string? TextValue { get; protected set; }

    public virtual string? TextEnglish { get; protected set; }

    public virtual decimal? ProdValue { get; protected set; }

    [External]
    public virtual decimal BCAAdjustedValue { get; private set; }
    [External]
    decimal IValue.BCAAdjustedValue { set => BCAAdjustedValue = value; }

    [External]
    public virtual bool HasErrors { get; private set; }
    [External]
    bool IValue.HasErrors
    {
        get => HasErrors;
        set => HasErrors = value;
    }

    public async Task<Value> CloneAsync(Item newOwner, ValueDef newDef)
    {
        var result = new Value(context)
        {
            Owner = newOwner,
            Def = newDef,

            NumberValue = NumberValue,
            TextValue = TextValue,
            ProdValue = ProdValue
        };

        await context.Persistence.PersistAsync(result);

        return result;
    }

    public virtual void UpdateData(ValueData data)
    {
        if (Def.Type == ValueType.NUMBER)
        {
            NumberValue = data.NumberValue;
            ProdValue = data.ProdValue;
            TextValue = null;
            TextEnglish = null;
        }
        else
        {
            NumberValue = null;
            ProdValue = null;
            TextValue = data.TextValue;
            TextEnglish = data.TextEnglish;
        }
    }

    public virtual async Task DeleteAsync()
    {
        Owner.RemoveValue(this);

        await context.Persistence.DeleteAsync(this);
    }
}

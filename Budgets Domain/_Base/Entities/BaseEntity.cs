using Krypton.Budgets.Domain._Base.Interfaces;
using Krypton.Budgets.Domain._Impl.Attributes;
using Krypton.Budgets.Domain._Impl.Interfaces;
using Krypton.Budgets.Domain._Impl.Utils;

namespace Krypton.Budgets.Domain._Base.Entities;

internal abstract class BaseEntity : IEntity
{
    protected readonly IContext context;

    protected BaseEntity()
    {
        context = UninitializedContet.Instance;
    }

    protected BaseEntity(IContext context)
    {
        this.context = context;
    }

    [Required]
    public virtual Guid Id { get; protected set; }

    [Required]
    public virtual DateTime? WhenCreated { get; protected set; }

    [Required]
    public virtual string? WhoCreated { get; protected set; }

    [Required]
    public virtual DateTime? WhenUpdated { get; protected set; }

    [Required]
    public virtual string? WhoUpdated { get; protected set; }
}

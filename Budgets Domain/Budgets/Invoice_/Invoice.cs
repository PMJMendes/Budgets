using Krypton.Budgets.Domain._Base.Entities;
using Krypton.Budgets.Domain._Impl.Attributes;
using Krypton.Budgets.Domain._Impl.Interfaces;
using Krypton.Budgets.Domain.Budgets.Item_;
using Krypton.Budgets.Shared;

namespace Krypton.Budgets.Domain.Budgets.Invoice_;

internal class Invoice : BaseEntity, IInvoice
{
    protected Invoice() { }

    protected Invoice(IContext context) : base(context) { }

    public Invoice(IContext context, Item owner, int order, InvoiceData data) : this(context)
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
    public virtual decimal InvoicedValue { get; protected set; }

    public string? InvoiceNumber { get; protected set; }
    public string? Supplier { get; protected set; }
    public string? Text { get; protected set; }

    public void ReRank(int i) => Order = i;

    public virtual void UpdateData(InvoiceData data)
    {
        InvoicedValue = data.InvoicedValue;
        InvoiceNumber = data.InvoiceNumber;
        Supplier = data.Supplier;
        Text = data.Text;
    }

    public virtual async Task DeleteAsync()
    {
        Owner.RemoveInvoice(this);

        await context.Persistence.DeleteAsync(this);
    }
}

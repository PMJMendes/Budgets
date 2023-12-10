namespace Krypton.Budgets.Domain.Budgets.Budget_.BudgetDetails;

public interface IItemItem
{
    Guid Id { get; }

    bool ExcludeFromBase { get; }
    bool CanBePercent { get; }
    string Description { get; }
    string? DescEnglish { get; }
    decimal? Percent { get; }
    decimal? BCAPercent { get; }

    IEnumerable<IValueItem> Values { get; }
    IEnumerable<ICostItem> Costs { get; }
    IEnumerable<IInvoiceItem> Invoices { get; }
}

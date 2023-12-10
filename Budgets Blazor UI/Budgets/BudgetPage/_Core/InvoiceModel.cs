using Krypton.Budgets.Blazor.APIClient.Budgets.Budget.BudgetDetails;
using Krypton.Budgets.Blazor.APIClient.Budgets.Budget.ManageBudget;
using Krypton.Budgets.Blazor.APIClient.Budgets.Budget.ProductionDetails;
using Krypton.Budgets.Shared;

namespace Krypton.Budgets.Blazor.UI.Budgets.BudgetPage._Core;

public class InvoiceModel : IInvoice
{
    private InvoiceModel(Guid id, decimal? invoicedValue, string? invoiceNumber, string? supplier, string? text)
    {
        Id = id;

        InvoicedValue = invoicedValue;
        InvoiceNumber = invoiceNumber;
        Supplier = supplier;
        Text = text;
    }

    public Guid Id { get; private init; }

    public decimal? InvoicedValue { get; set; }
    public string? InvoiceNumber { get; set; }
    public string? Supplier { get; set; }
    public string? Text { get; set; }

    decimal IInvoice.InvoicedValue => InvoicedValue ?? 0m;

    public MInvoiceArgs AsManageArgs() => new(
        Id,
        InvoicedValue ?? 0m,
        InvoiceNumber,
        Supplier,
        Text
    );

    public InvoiceModel Clone() => new(
        Id,
        InvoicedValue,
        InvoiceNumber,
        Supplier,
        Text
    );

    public static InvoiceModel Empty() => new(
        Guid.Empty,
        0m,
        null,
        null,
        null
    );

    public static InvoiceModel FromData(decimal? invoicedValue, string? invoiceNumber, string? supplier, string? text) => new(
        Guid.Empty,
        invoicedValue,
        invoiceNumber,
        supplier,
        text
    );

    public static InvoiceModel FromService(InvoiceItem? item) => new(
        item?.Id ?? Guid.Empty,
        item?.InvoicedValue ?? 0m,
        item?.InvoiceNumber,
        item?.Supplier,
        item?.Text
    );

    public static InvoiceModel FromService(PInvoiceItem? item) => new(
        item?.Id ?? Guid.Empty,
        item?.InvoicedValue ?? 0m,
        item?.InvoiceNumber,
        item?.Supplier,
        item?.Text
    );
}

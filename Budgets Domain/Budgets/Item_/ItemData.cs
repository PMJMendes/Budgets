using Krypton.Budgets.Domain.Budgets.Cost_;
using Krypton.Budgets.Domain.Budgets.Invoice_;
using Krypton.Budgets.Domain.Budgets.Value_;

namespace Krypton.Budgets.Domain.Budgets.Item_;

internal record struct ItemData(
    decimal? Percent,
    decimal? BCAPercent,
    IEnumerable<ValueData> ValueData,
    IEnumerable<CostData> CostData,
    IEnumerable<InvoiceData> InvoiceData);

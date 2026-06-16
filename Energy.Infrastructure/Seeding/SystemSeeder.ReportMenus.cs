using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Energy.Infrastructure.Seeding;

/// <summary>
/// Per-report menü tohumlaması: her rapor, modül menüsünün altına
/// /{module}/reports/{report} rotasıyla ve {Module}.{Report}.Read yetkisiyle eklenir.
/// </summary>
public sealed partial class SystemSeeder
{
    /// <summary>(Module, ParentMenuNameKey, Report, Route, NameKey, Order)</summary>
    private static readonly (string Module, string ParentKey, string Report, string Route, string NameKey, int Order)[] ModuleReportMenus =
    [
        ("Procurement", "Menus.Procurement", "PurchaseOrderSummary", "/procurement/reports/purchase-order-summary", "Menus.Procurement.Reports.PurchaseOrderSummary", 1),
        ("Inventory", "Menus.Inventory", "StockBalanceReport", "/inventory/reports/stock-balance-report", "Menus.Inventory.Reports.StockBalanceReport", 2),
        ("Projects", "Menus.Projects", "ProjectStatusReport", "/projects/reports/project-status-report", "Menus.Projects.Reports.ProjectStatusReport", 3),
        ("HR", "Menus.HR", "TimesheetSummary", "/h-r/reports/timesheet-summary", "Menus.HR.Reports.TimesheetSummary", 4),
        ("Finance", "Menus.Finance", "PayableAging", "/finance/reports/payable-aging", "Menus.Finance.Reports.PayableAging", 5),
        ("Finance", "Menus.Finance", "ReceivableAging", "/finance/reports/receivable-aging", "Menus.Finance.Reports.ReceivableAging", 6),
        ("ProgressPayments", "Menus.ProgressPayments", "ProgressPaymentSummary", "/progress-payments/reports/progress-payment-summary", "Menus.ProgressPayments.Reports.ProgressPaymentSummary", 7),
    ];

    /// <summary>Modül menüsünün altına per-report menü girdilerini idempotent ekler.</summary>
    private async Task EnsureReportMenusAsync(CancellationToken ct)
    {
        foreach (var (module, parentKey, report, route, nameKey, order) in ModuleReportMenus)
        {
            var parent = await _db.Menus.FirstOrDefaultAsync(m => m.NameKey == parentKey, ct);
            if (parent is null)
            {
                continue;
            }
            await EnsureMenuAsync(nameKey, parent.Id, route, "chart", 300 + order, $"{module}.{report}.Read", ct);
        }
        _logger.LogInformation("Seeding: {Count} per-report menu(s) ensured.", ModuleReportMenus.Length);
    }
}

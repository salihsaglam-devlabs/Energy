using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Energy.Infrastructure.Seeding;

/// <summary>
/// Per-process menü tohumlaması: standart süreç rotalarına karşılık gelen menü
/// girdileri ilgili modül menüsünün altına idempotent eklenir.
/// </summary>
public sealed partial class SystemSeeder
{
    /// <summary>(Module, ParentMenuNameKey, Process, Route, NameKey, Permission, Order)</summary>
    private static readonly (string Module, string ParentKey, string Process, string Route, string NameKey, string Permission, int Order)[] ModuleProcessMenus =
    [
        ("Workflow", "Menus.Workflow", "Approval", "/workflow/processes/approval", "Menus.Workflow.Processes.Approval", "Workflow.Read", 1),
        ("Inventory", "Menus.Inventory", "StockIssue", "/inventory/processes/stock-issue", "Menus.Inventory.Processes.StockIssue", "Inventory.Approve", 2),
        ("Inventory", "Menus.Inventory", "StockTransfer", "/inventory/processes/stock-transfer", "Menus.Inventory.Processes.StockTransfer", "Inventory.Transfer", 3),
        ("Procurement", "Menus.Procurement", "GoodsReceipt", "/procurement/processes/goods-receipt", "Menus.Procurement.Processes.GoodsReceipt", "Procurement.Approve", 4),
        ("Finance", "Menus.Finance", "TimesheetCost", "/finance/processes/timesheet-cost", "Menus.Finance.Processes.TimesheetCost", "Finance.Create", 5),
        ("Finance", "Menus.Finance", "ProgressPaymentPosting", "/finance/processes/progress-payment-posting", "Menus.Finance.Processes.ProgressPaymentPosting", "Finance.Create", 6),
        ("Finance", "Menus.Finance", "PaymentAllocation", "/finance/processes/payment-allocation", "Menus.Finance.Processes.PaymentAllocation", "Finance.Update", 7),
        ("Documents", "Menus.Documents", "Files", "/documents/files", "Menus.Documents.Files", "Documents.Read", 7),
    ];

    /// <summary>Modül menüsünün altına per-process menü girdilerini idempotent ekler.</summary>
    private async Task EnsureModulesProcessMenusAsync(CancellationToken ct)
    {
        foreach (var (_, parentKey, _, route, nameKey, permission, order) in ModuleProcessMenus)
        {
            var parent = await _db.Menus.FirstOrDefaultAsync(m => m.NameKey == parentKey, ct);
            if (parent is null)
            {
                continue;
            }
            await EnsureMenuAsync(nameKey, parent.Id, route, "todo", 200 + order, permission, ct);
        }
        _logger.LogInformation("Seeding: {Count} per-process menu(s) ensured.", ModuleProcessMenus.Length);
    }
}


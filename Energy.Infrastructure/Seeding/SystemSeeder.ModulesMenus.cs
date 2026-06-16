using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Energy.Infrastructure.Seeding;

/// <summary>
/// Per-entity menü tohumlaması: her web-yönetimli entity, modül menüsünün altına
/// /{module}/{entity} rotasıyla ve {Module}.ReadAll yetkisiyle idempotent eklenir.
/// </summary>
public sealed partial class SystemSeeder
{
    /// <summary>(Module, ParentMenuNameKey, Entity, Route, NameKey, Order)</summary>
    private static readonly (string Module, string ParentKey, string Entity, string Route, string NameKey, int Order)[] ModuleEntityMenus =
    [
        ("Core", "Menus.CoreData", "Company", "/core/companies", "Menus.Core.Company", 1),
        ("Core", "Menus.CoreData", "Branch", "/core/branches", "Menus.Core.Branch", 2),
        ("Core", "Menus.CoreData", "Department", "/core/departments", "Menus.Core.Department", 3),
        ("Core", "Menus.CoreData", "Currency", "/core/currencies", "Menus.Core.Currency", 4),
        ("Core", "Menus.CoreData", "ExchangeRate", "/core/exchange-rates", "Menus.Core.ExchangeRate", 5),
        ("Core", "Menus.CoreData", "UnitOfMeasure", "/core/units-of-measure", "Menus.Core.UnitOfMeasure", 6),
        ("Core", "Menus.CoreData", "UnitConversion", "/core/unit-conversions", "Menus.Core.UnitConversion", 7),
        ("Core", "Menus.CoreData", "SequenceDefinition", "/core/sequence-definitions", "Menus.Core.SequenceDefinition", 8),
        ("Core", "Menus.CoreData", "SystemSetting", "/core/system-settings", "Menus.Core.SystemSetting", 9),
        ("Core", "Menus.CoreData", "LocalizationResource", "/core/localization-resources", "Menus.Core.LocalizationResource", 10),
        ("Core", "Menus.CoreData", "AuditLog", "/core/audit-logs", "Menus.Core.AuditLog", 11),
        ("Organization", "Menus.Organization", "Employee", "/organization/employees", "Menus.Organization.Employee", 1),
        ("Organization", "Menus.Organization", "EmployeePosition", "/organization/employee-positions", "Menus.Organization.EmployeePosition", 2),
        ("Organization", "Menus.Organization", "EmployeeSkill", "/organization/employee-skills", "Menus.Organization.EmployeeSkill", 3),
        ("Organization", "Menus.Organization", "EmployeeSkillAssignment", "/organization/employee-skill-assignments", "Menus.Organization.EmployeeSkillAssignment", 4),
        ("Organization", "Menus.Organization", "LeaveRequest", "/organization/leave-requests", "Menus.Organization.LeaveRequest", 5),
        ("Organization", "Menus.Organization", "ExpenseClaim", "/organization/expense-claims", "Menus.Organization.ExpenseClaim", 6),
        ("Organization", "Menus.Organization", "ExpenseClaimLine", "/organization/expense-claim-lines", "Menus.Organization.ExpenseClaimLine", 7),
        ("BusinessPartners", "Menus.BusinessPartners", "BusinessPartner", "/business-partners/business-partners", "Menus.BusinessPartners.BusinessPartner", 1),
        ("BusinessPartners", "Menus.BusinessPartners", "BusinessPartnerContact", "/business-partners/business-partner-contacts", "Menus.BusinessPartners.BusinessPartnerContact", 2),
        ("BusinessPartners", "Menus.BusinessPartners", "BusinessPartnerAddress", "/business-partners/business-partner-addresses", "Menus.BusinessPartners.BusinessPartnerAddress", 3),
        ("BusinessPartners", "Menus.BusinessPartners", "BusinessPartnerBankAccount", "/business-partners/business-partner-bank-accounts", "Menus.BusinessPartners.BusinessPartnerBankAccount", 4),
        ("Projects", "Menus.Projects", "Project", "/projects/projects", "Menus.Projects.Project", 1),
        ("Projects", "Menus.Projects", "ProjectType", "/projects/project-types", "Menus.Projects.ProjectType", 2),
        ("Projects", "Menus.Projects", "ProjectStatus", "/projects/project-statuses", "Menus.Projects.ProjectStatus", 3),
        ("Projects", "Menus.Projects", "ProjectLocation", "/projects/project-locations", "Menus.Projects.ProjectLocation", 4),
        ("Projects", "Menus.Projects", "ProjectPhas", "/projects/project-phases", "Menus.Projects.ProjectPhas", 5),
        ("Projects", "Menus.Projects", "ProjectMember", "/projects/project-members", "Menus.Projects.ProjectMember", 6),
        ("Projects", "Menus.Projects", "ProjectNote", "/projects/project-notes", "Menus.Projects.ProjectNote", 7),
        ("Catalog", "Menus.Catalog", "Brand", "/catalog/brands", "Menus.Catalog.Brand", 1),
        ("Catalog", "Menus.Catalog", "MaterialCategory", "/catalog/material-categories", "Menus.Catalog.MaterialCategory", 2),
        ("Catalog", "Menus.Catalog", "MaterialAttributeDefinition", "/catalog/material-attribute-definitions", "Menus.Catalog.MaterialAttributeDefinition", 3),
        ("Catalog", "Menus.Catalog", "MaterialAttributeOption", "/catalog/material-attribute-options", "Menus.Catalog.MaterialAttributeOption", 4),
        ("Catalog", "Menus.Catalog", "MaterialCategoryAttribute", "/catalog/material-category-attributes", "Menus.Catalog.MaterialCategoryAttribute", 5),
        ("Catalog", "Menus.Catalog", "Material", "/catalog/materials", "Menus.Catalog.Material", 6),
        ("Catalog", "Menus.Catalog", "MaterialAttributeValue", "/catalog/material-attribute-values", "Menus.Catalog.MaterialAttributeValue", 7),
        ("Catalog", "Menus.Catalog", "MaterialUnitConversion", "/catalog/material-unit-conversions", "Menus.Catalog.MaterialUnitConversion", 8),
        ("Inventory", "Menus.Inventory", "Warehouse", "/inventory/warehouses", "Menus.Inventory.Warehouse", 1),
        ("Inventory", "Menus.Inventory", "WarehouseLocation", "/inventory/warehouse-locations", "Menus.Inventory.WarehouseLocation", 2),
        ("Inventory", "Menus.Inventory", "StockDocumentType", "/inventory/stock-document-types", "Menus.Inventory.StockDocumentType", 3),
        ("Inventory", "Menus.Inventory", "StockDocument", "/inventory/stock-documents", "Menus.Inventory.StockDocument", 4),
        ("Inventory", "Menus.Inventory", "StockDocumentLine", "/inventory/stock-document-lines", "Menus.Inventory.StockDocumentLine", 5),
        ("Inventory", "Menus.Inventory", "StockLot", "/inventory/stock-lots", "Menus.Inventory.StockLot", 6),
        ("Inventory", "Menus.Inventory", "StockIssueAllocation", "/inventory/stock-issue-allocations", "Menus.Inventory.StockIssueAllocation", 7),
        ("Inventory", "Menus.Inventory", "StockTransaction", "/inventory/stock-transactions", "Menus.Inventory.StockTransaction", 8),
        ("Inventory", "Menus.Inventory", "StockBalance", "/inventory/stock-balances", "Menus.Inventory.StockBalance", 9),
        ("Inventory", "Menus.Inventory", "StockReservation", "/inventory/stock-reservations", "Menus.Inventory.StockReservation", 10),
        ("Inventory", "Menus.Inventory", "StockCount", "/inventory/stock-counts", "Menus.Inventory.StockCount", 11),
        ("Inventory", "Menus.Inventory", "StockCountLine", "/inventory/stock-count-lines", "Menus.Inventory.StockCountLine", 12),
        ("Inventory", "Menus.Inventory", "WarehouseTransfer", "/inventory/warehouse-transfers", "Menus.Inventory.WarehouseTransfer", 13),
        ("Inventory", "Menus.Inventory", "WarehouseTransferLine", "/inventory/warehouse-transfer-lines", "Menus.Inventory.WarehouseTransferLine", 14),
        ("Requests", "Menus.Requests", "RequestType", "/requests/request-types", "Menus.Requests.RequestType", 1),
        ("Requests", "Menus.Requests", "Request", "/requests/requests", "Menus.Requests.Request", 2),
        ("Requests", "Menus.Requests", "RequestLine", "/requests/request-lines", "Menus.Requests.RequestLine", 3),
        ("Procurement", "Menus.Procurement", "SupplierQuote", "/procurement/supplier-quotes", "Menus.Procurement.SupplierQuote", 1),
        ("Procurement", "Menus.Procurement", "SupplierQuoteLine", "/procurement/supplier-quote-lines", "Menus.Procurement.SupplierQuoteLine", 2),
        ("Procurement", "Menus.Procurement", "PurchaseOrder", "/procurement/purchase-orders", "Menus.Procurement.PurchaseOrder", 3),
        ("Procurement", "Menus.Procurement", "PurchaseOrderLine", "/procurement/purchase-order-lines", "Menus.Procurement.PurchaseOrderLine", 4),
        ("Procurement", "Menus.Procurement", "PurchaseReceipt", "/procurement/purchase-receipts", "Menus.Procurement.PurchaseReceipt", 5),
        ("Procurement", "Menus.Procurement", "PurchaseReceiptLine", "/procurement/purchase-receipt-lines", "Menus.Procurement.PurchaseReceiptLine", 6),
        ("Procurement", "Menus.Procurement", "SupplierInvoice", "/procurement/supplier-invoices", "Menus.Procurement.SupplierInvoice", 7),
        ("Procurement", "Menus.Procurement", "SupplierInvoiceLine", "/procurement/supplier-invoice-lines", "Menus.Procurement.SupplierInvoiceLine", 8),
        ("Operations", "Menus.Operations", "WorkOrderType", "/operations/work-order-types", "Menus.Operations.WorkOrderType", 1),
        ("Operations", "Menus.Operations", "WorkOrder", "/operations/work-orders", "Menus.Operations.WorkOrder", 2),
        ("Operations", "Menus.Operations", "WorkOrderAssignment", "/operations/work-order-assignments", "Menus.Operations.WorkOrderAssignment", 3),
        ("Operations", "Menus.Operations", "WorkOrderMaterialPlan", "/operations/work-order-material-plans", "Menus.Operations.WorkOrderMaterialPlan", 4),
        ("Operations", "Menus.Operations", "WorkOrderMaterialUsage", "/operations/work-order-material-usages", "Menus.Operations.WorkOrderMaterialUsage", 5),
        ("Operations", "Menus.Operations", "WorkOrderChecklist", "/operations/work-order-checklists", "Menus.Operations.WorkOrderChecklist", 6),
        ("Operations", "Menus.Operations", "WorkOrderChecklistItem", "/operations/work-order-checklist-items", "Menus.Operations.WorkOrderChecklistItem", 7),
        ("Operations", "Menus.Operations", "WorkOrderStatusHistory", "/operations/work-order-status-histories", "Menus.Operations.WorkOrderStatusHistory", 8),
        ("FieldOperations", "Menus.FieldOperations", "DailySiteReport", "/field-operations/daily-site-reports", "Menus.FieldOperations.DailySiteReport", 1),
        ("FieldOperations", "Menus.FieldOperations", "DailySiteReportWorker", "/field-operations/daily-site-report-workers", "Menus.FieldOperations.DailySiteReportWorker", 2),
        ("FieldOperations", "Menus.FieldOperations", "DailySiteReportEquipment", "/field-operations/daily-site-report-equipments", "Menus.FieldOperations.DailySiteReportEquipment", 3),
        ("FieldOperations", "Menus.FieldOperations", "DailySiteReportMaterial", "/field-operations/daily-site-report-materials", "Menus.FieldOperations.DailySiteReportMaterial", 4),
        ("FieldOperations", "Menus.FieldOperations", "ProgressEntry", "/field-operations/progress-entries", "Menus.FieldOperations.ProgressEntry", 5),
        ("FieldOperations", "Menus.FieldOperations", "MeasurementSheet", "/field-operations/measurement-sheets", "Menus.FieldOperations.MeasurementSheet", 6),
        ("FieldOperations", "Menus.FieldOperations", "MeasurementSheetLine", "/field-operations/measurement-sheet-lines", "Menus.FieldOperations.MeasurementSheetLine", 7),
        ("HR", "Menus.HR", "Timesheet", "/h-r/timesheets", "Menus.HR.Timesheet", 1),
        ("HR", "Menus.HR", "TimesheetLine", "/h-r/timesheet-lines", "Menus.HR.TimesheetLine", 2),
        ("Assets", "Menus.Assets", "EquipmentAsset", "/assets/equipment-assets", "Menus.Assets.EquipmentAsset", 1),
        ("Assets", "Menus.Assets", "EquipmentAssignment", "/assets/equipment-assignments", "Menus.Assets.EquipmentAssignment", 2),
        ("Assets", "Menus.Assets", "EquipmentMaintenance", "/assets/equipment-maintenances", "Menus.Assets.EquipmentMaintenance", 3),
        ("Finance", "Menus.Finance", "FinancialAccount", "/finance/financial-accounts", "Menus.Finance.FinancialAccount", 1),
        ("Finance", "Menus.Finance", "CostCenter", "/finance/cost-centers", "Menus.Finance.CostCenter", 2),
        ("Finance", "Menus.Finance", "FinancialTransaction", "/finance/financial-transactions", "Menus.Finance.FinancialTransaction", 3),
        ("Finance", "Menus.Finance", "FinancialTransactionLine", "/finance/financial-transaction-lines", "Menus.Finance.FinancialTransactionLine", 4),
        ("Finance", "Menus.Finance", "Payable", "/finance/payables", "Menus.Finance.Payable", 5),
        ("Finance", "Menus.Finance", "Receivable", "/finance/receivables", "Menus.Finance.Receivable", 6),
        ("Finance", "Menus.Finance", "Payment", "/finance/payments", "Menus.Finance.Payment", 7),
        ("Finance", "Menus.Finance", "PaymentAllocation", "/finance/payment-allocations", "Menus.Finance.PaymentAllocation", 8),
        ("Finance", "Menus.Finance", "Collection", "/finance/collections", "Menus.Finance.Collection", 9),
        ("Finance", "Menus.Finance", "CollectionAllocation", "/finance/collection-allocations", "Menus.Finance.CollectionAllocation", 10),
        ("Budget", "Menus.Budget", "Budget", "/budget/budgets", "Menus.Budget.Budget", 1),
        ("Budget", "Menus.Budget", "BudgetLine", "/budget/budget-lines", "Menus.Budget.BudgetLine", 2),
        ("Contracts", "Menus.Contracts", "Contract", "/contracts/contracts", "Menus.Contracts.Contract", 1),
        ("Contracts", "Menus.Contracts", "ContractParty", "/contracts/contract-parties", "Menus.Contracts.ContractParty", 2),
        ("Contracts", "Menus.Contracts", "ContractLine", "/contracts/contract-lines", "Menus.Contracts.ContractLine", 3),
        ("Contracts", "Menus.Contracts", "ContractAmendment", "/contracts/contract-amendments", "Menus.Contracts.ContractAmendment", 4),
        ("ProgressPayments", "Menus.ProgressPayments", "ProgressPayment", "/progress-payments/progress-payments", "Menus.ProgressPayments.ProgressPayment", 1),
        ("ProgressPayments", "Menus.ProgressPayments", "ProgressPaymentLine", "/progress-payments/progress-payment-lines", "Menus.ProgressPayments.ProgressPaymentLine", 2),
        ("ProgressPayments", "Menus.ProgressPayments", "ProgressPaymentDeduction", "/progress-payments/progress-payment-deductions", "Menus.ProgressPayments.ProgressPaymentDeduction", 3),
        ("Documents", "Menus.Documents", "DocumentFolder", "/documents/document-folders", "Menus.Documents.DocumentFolder", 1),
        ("Documents", "Menus.Documents", "Document", "/documents/documents", "Menus.Documents.Document", 2),
        ("Documents", "Menus.Documents", "DocumentVersion", "/documents/document-versions", "Menus.Documents.DocumentVersion", 3),
        ("Documents", "Menus.Documents", "DocumentRelation", "/documents/document-relations", "Menus.Documents.DocumentRelation", 4),
        ("Documents", "Menus.Documents", "DocumentPermission", "/documents/document-permissions", "Menus.Documents.DocumentPermission", 5),
        ("Workflow", "Menus.Workflow", "ApprovalDefinition", "/workflow/approval-definitions", "Menus.Workflow.ApprovalDefinition", 1),
        ("Workflow", "Menus.Workflow", "ApprovalDefinitionVersion", "/workflow/approval-definition-versions", "Menus.Workflow.ApprovalDefinitionVersion", 2),
        ("Workflow", "Menus.Workflow", "ApprovalStepDefinition", "/workflow/approval-step-definitions", "Menus.Workflow.ApprovalStepDefinition", 3),
        ("Workflow", "Menus.Workflow", "ApprovalStepApprover", "/workflow/approval-step-approvers", "Menus.Workflow.ApprovalStepApprover", 4),
        ("Workflow", "Menus.Workflow", "ApprovalCondition", "/workflow/approval-conditions", "Menus.Workflow.ApprovalCondition", 5),
        ("Workflow", "Menus.Workflow", "ApprovalRequest", "/workflow/approval-requests", "Menus.Workflow.ApprovalRequest", 6),
        ("Workflow", "Menus.Workflow", "ApprovalRequestStep", "/workflow/approval-request-steps", "Menus.Workflow.ApprovalRequestStep", 7),
        ("Workflow", "Menus.Workflow", "ApprovalRequestApprover", "/workflow/approval-request-approvers", "Menus.Workflow.ApprovalRequestApprover", 8),
        ("Workflow", "Menus.Workflow", "ApprovalAction", "/workflow/approval-actions", "Menus.Workflow.ApprovalAction", 9),
        ("Workflow", "Menus.Workflow", "ApprovalDelegation", "/workflow/approval-delegations", "Menus.Workflow.ApprovalDelegation", 10),
        ("Notifications", "Menus.Notifications", "Notification", "/notifications/notifications", "Menus.Notifications.Notification", 1),
        ("Notifications", "Menus.Notifications", "NotificationRecipient", "/notifications/notification-recipients", "Menus.Notifications.NotificationRecipient", 2),
        ("Notifications", "Menus.Notifications", "NotificationPreference", "/notifications/notification-preferences", "Menus.Notifications.NotificationPreference", 3),
        ("Reporting", "Menus.Reporting", "ReportDefinition", "/reporting/report-definitions", "Menus.Reporting.ReportDefinition", 1),
        ("Reporting", "Menus.Reporting", "DashboardWidget", "/reporting/dashboard-widgets", "Menus.Reporting.DashboardWidget", 2),
    ];

    /// <summary>Modül menüsünün altına per-entity menü girdilerini idempotent ekler.</summary>
    private async Task EnsureModulesEntityMenusAsync(CancellationToken ct)
    {
        foreach (var (module, parentKey, _, route, nameKey, order) in ModuleEntityMenus)
        {
            var parent = await _db.Menus.FirstOrDefaultAsync(m => m.NameKey == parentKey, ct);
            if (parent is null)
            {
                continue;
            }
            await EnsureMenuAsync(nameKey, parent.Id, route, "doc", 100 + order, $"{module}.ReadAll", ct);
        }
        _logger.LogInformation("Seeding: {Count} per-entity module menu(s) ensured.", ModuleEntityMenus.Length);
    }
}

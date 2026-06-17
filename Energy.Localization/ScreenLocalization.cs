// ScreenLocalization: ekran bazlı (per-screen) yerelleştirme altyapısı.
//
// Her ekranın TÜM kullanıcıya açık metinleri kendi bölümünde (resx içinde
// "{ScreenId}.*" anahtarlarıyla) tanımlıdır. Bu sınıf, verilen ekran kimliği
// için o ekrana ait sözlüğü (başlık, açıklama, kolon adları, aksiyonlar, grid
// metinleri, bildirimler, popup başlıkları, validasyon, onay) tek seferde
// oluşturur. Ortak anahtar kullanılmaz; her ekran kendi karşılıklarını taşır.
using System.Collections.Generic;
using Microsoft.Extensions.Localization;

namespace Energy.Localization;

public static class ScreenLocalization
{
    // Ekran kimliği -> o ekranın DataGrid kolon alan adları (DTO sırasına göre).
    // resx'te "{ScreenId}.Columns.{field}" anahtarı bu listeden çözülür.
    public static readonly IReadOnlyDictionary<string, string[]> Registry =
        new Dictionary<string, string[]>(System.StringComparer.OrdinalIgnoreCase)
    {
        ["Assets.EquipmentAsset"] = new[] { "companyId", "code", "name", "assetType", "serialNo", "purchaseDate", "isActive" },
        ["Assets.EquipmentAssignment"] = new[] { "equipmentAssetId", "projectId", "employeeId", "warehouseId", "startDate", "endDate", "isActive" },
        ["Assets.EquipmentMaintenance"] = new[] { "equipmentAssetId", "maintenanceType", "scheduledDate", "completedDate", "cost", "note" },
        ["Budget.Budget"] = new[] { "projectId", "costCenterId", "currencyId", "name", "year", "isActive" },
        ["Budget.BudgetLine"] = new[] { "budgetId", "projectId", "costCenterId", "description", "plannedAmount" },
        ["BusinessPartners.BusinessPartner"] = new[] { "partnerType", "code", "name", "taxNumber", "taxOffice", "phone", "email", "isActive" },
        ["BusinessPartners.BusinessPartnerAddress"] = new[] { "businessPartnerId", "addressType", "addressLine", "city", "country", "postalCode", "isPrimary" },
        ["BusinessPartners.BusinessPartnerBankAccount"] = new[] { "businessPartnerId", "bankName", "branch", "iban", "currencyId", "isPrimary" },
        ["BusinessPartners.BusinessPartnerContact"] = new[] { "businessPartnerId", "fullName", "title", "phone", "email", "isPrimary" },
        ["Catalog.Brand"] = new[] { "code", "name", "isActive" },
        ["Catalog.Material"] = new[] { "materialCategoryId", "brandId", "baseUnitOfMeasureId", "code", "name", "isBatchTracked", "isSerialTracked", "isActive" },
        ["Catalog.MaterialAttributeDefinition"] = new[] { "code", "name", "dataType", "isActive" },
        ["Catalog.MaterialAttributeOption"] = new[] { "materialAttributeDefinitionId", "value", "displayOrder" },
        ["Catalog.MaterialAttributeValue"] = new[] { "materialId", "materialAttributeDefinitionId", "optionId", "valueText", "valueNumber", "valueBoolean", "valueDate" },
        ["Catalog.MaterialCategory"] = new[] { "parentCategoryId", "code", "name", "isActive" },
        ["Catalog.MaterialCategoryAttribute"] = new[] { "materialCategoryId", "materialAttributeDefinitionId", "isRequired", "displayOrder" },
        ["Catalog.MaterialUnitConversion"] = new[] { "materialId", "fromUnitOfMeasureId", "toUnitOfMeasureId", "factor" },
        ["Contracts.Contract"] = new[] { "contractType", "projectId", "contractNo", "currencyId", "contractAmount", "title", "startDate", "endDate", "status" },
        ["Contracts.ContractAmendment"] = new[] { "contractId", "amendmentNo", "amendmentDate", "description", "amountDelta" },
        ["Contracts.ContractLine"] = new[] { "contractId", "description", "quantity", "unitPrice" },
        ["Contracts.ContractParty"] = new[] { "contractId", "businessPartnerId", "partyRole" },
        ["Core.AuditLog"] = new[] { "occurredAt", "userId", "userName", "ipAddress", "httpMethod", "path", "queryString", "statusCode", "isSuccess", "source", "requestBody", "responseBody", "hasException", "exceptionType", "exceptionMessage", "correlationId", "durationMs" },
        ["Core.Branch"] = new[] { "companyId", "code", "name", "address", "isActive" },
        ["Core.Company"] = new[] { "code", "name", "baseCurrencyId", "taxNumber", "address", "isActive" },
        ["Core.Currency"] = new[] { "code", "name", "symbol", "isActive" },
        ["Core.Department"] = new[] { "companyId", "parentDepartmentId", "code", "name", "managerUserId", "isActive" },
        ["Core.ExchangeRate"] = new[] { "currencyId", "rateDate", "rate" },
        ["Core.LocalizationResource"] = new[] { "key", "culture", "value" },
        ["Core.SequenceDefinition"] = new[] { "module", "entityType", "prefix", "padding", "nextNumber", "format" },
        ["Core.SystemSetting"] = new[] { "key", "value", "category", "descriptionKey" },
        ["Core.UnitConversion"] = new[] { "fromUnitOfMeasureId", "toUnitOfMeasureId", "factor" },
        ["Core.UnitOfMeasure"] = new[] { "code", "name", "symbol", "isActive" },
        ["Documents.Document"] = new[] { "documentFolderId", "name", "description", "status", "currentVersionNo" },
        ["Documents.DocumentFolder"] = new[] { "parentFolderId", "name" },
        ["Documents.DocumentPermission"] = new[] { "documentId", "userId", "roleId", "accessType" },
        ["Documents.DocumentRelation"] = new[] { "documentId", "relatedModule", "relatedEntityType", "relatedEntityId" },
        ["Documents.DocumentVersion"] = new[] { "documentId", "versionNo", "fileName", "filePath", "fileSize", "contentType", "uploadedAt" },
        ["FieldOperations.DailySiteReport"] = new[] { "projectId", "workOrderId", "reportNo", "reportDate", "weather", "notes", "status" },
        ["FieldOperations.DailySiteReportEquipment"] = new[] { "dailySiteReportId", "equipmentAssetId", "equipmentText", "hours" },
        ["FieldOperations.DailySiteReportMaterial"] = new[] { "dailySiteReportId", "materialId", "quantity" },
        ["FieldOperations.DailySiteReportWorker"] = new[] { "dailySiteReportId", "employeeId", "hoursWorked", "note" },
        ["FieldOperations.MeasurementSheet"] = new[] { "projectId", "contractId", "sheetNo", "sheetDate", "status" },
        ["FieldOperations.MeasurementSheetLine"] = new[] { "measurementSheetId", "description", "quantity", "unitPrice" },
        ["FieldOperations.ProgressEntry"] = new[] { "projectId", "projectPhaseId", "entryDate", "quantity", "percentage", "note" },
        ["Finance.Collection"] = new[] { "partnerId", "currencyId", "financialAccountId", "amount", "collectionDate", "collectionNo", "status", "approvalRequestId" },
        ["Finance.CollectionAllocation"] = new[] { "collectionId", "receivableId", "amount" },
        ["Finance.CostCenter"] = new[] { "parentCostCenterId", "code", "name", "isActive" },
        ["Finance.FinancialAccount"] = new[] { "code", "name", "accountType", "currencyId", "isActive" },
        ["Finance.FinancialTransaction"] = new[] { "transactionType", "projectId", "partnerId", "currencyId", "amount", "relatedModule", "relatedEntityType", "relatedEntityId", "financialAccountId", "costCenterId", "transactionDate", "description", "isReversed" },
        ["Finance.FinancialTransactionLine"] = new[] { "financialTransactionId", "costCenterId", "projectId", "amount", "description" },
        ["Finance.Payable"] = new[] { "partnerId", "currencyId", "amount", "remainingAmount", "dueDate", "relatedModule", "relatedEntityType", "relatedEntityId", "isClosed" },
        ["Finance.Payment"] = new[] { "partnerId", "currencyId", "financialAccountId", "amount", "paymentDate", "paymentNo", "status", "approvalRequestId" },
        ["Finance.PaymentAllocation"] = new[] { "paymentId", "payableId", "amount" },
        ["Finance.Receivable"] = new[] { "partnerId", "currencyId", "amount", "remainingAmount", "dueDate", "relatedModule", "relatedEntityType", "relatedEntityId", "isClosed" },
        ["HR.Timesheet"] = new[] { "timesheetNo", "periodStart", "periodEnd", "status", "approvalRequestId" },
        ["HR.TimesheetLine"] = new[] { "timesheetId", "employeeId", "projectId", "workOrderId", "workDate", "normalHours", "overtimeHours", "hourlyCost" },
        ["Inventory.StockBalance"] = new[] { "warehouseId", "materialId", "quantity", "reservedQuantity", "totalCost", "lastRecalculatedAt" },
        ["Inventory.StockCount"] = new[] { "warehouseId", "countNo", "countDate", "status" },
        ["Inventory.StockCountLine"] = new[] { "stockCountId", "materialId", "systemQuantity", "countedQuantity" },
        ["Inventory.StockDocument"] = new[] { "documentTypeId", "sourceWarehouseId", "targetWarehouseId", "projectId", "status", "documentNo", "documentDate", "note", "approvalRequestId" },
        ["Inventory.StockDocumentLine"] = new[] { "stockDocumentId", "materialId", "unitOfMeasureId", "quantity", "unitPrice", "currencyId", "note" },
        ["Inventory.StockDocumentType"] = new[] { "code", "name", "direction", "isActive" },
        ["Inventory.StockIssueAllocation"] = new[] { "stockDocumentLineId", "stockLotId", "quantity", "unitCost" },
        ["Inventory.StockLot"] = new[] { "warehouseId", "materialId", "sourceStockDocumentLineId", "lotNo", "initialQuantity", "remainingQuantity", "unitCost", "receivedAt" },
        ["Inventory.StockReservation"] = new[] { "warehouseId", "materialId", "quantity", "relatedModule", "relatedEntityType", "relatedEntityId", "isReleased" },
        ["Inventory.StockTransaction"] = new[] { "stockDocumentId", "stockDocumentLineId", "stockLotId", "warehouseId", "materialId", "quantity", "unitCost", "transactionDate" },
        ["Inventory.Warehouse"] = new[] { "companyId", "branchId", "projectId", "warehouseType", "code", "name", "isActive" },
        ["Inventory.WarehouseLocation"] = new[] { "warehouseId", "parentLocationId", "code", "name" },
        ["Inventory.WarehouseTransfer"] = new[] { "sourceWarehouseId", "targetWarehouseId", "transferNo", "transferDate", "status" },
        ["Inventory.WarehouseTransferLine"] = new[] { "warehouseTransferId", "materialId", "quantity" },
        ["Notifications.Notification"] = new[] { "title", "body", "notificationType", "relatedModule", "relatedEntityType", "relatedEntityId" },
        ["Notifications.NotificationPreference"] = new[] { "userId", "notificationType", "inAppEnabled", "emailEnabled" },
        ["Notifications.NotificationRecipient"] = new[] { "notificationId", "userId", "isRead", "readAt" },
        ["Operations.WorkOrder"] = new[] { "workOrderTypeId", "projectId", "projectPhaseId", "projectLocationId", "status", "workOrderNo", "title", "description", "plannedStart", "plannedEnd" },
        ["Operations.WorkOrderAssignment"] = new[] { "workOrderId", "employeeId", "userId", "assignmentRole" },
        ["Operations.WorkOrderChecklist"] = new[] { "workOrderId", "name", "isRequired" },
        ["Operations.WorkOrderChecklistItem"] = new[] { "workOrderChecklistId", "description", "isRequired", "isCompleted" },
        ["Operations.WorkOrderMaterialPlan"] = new[] { "workOrderId", "materialId", "plannedQuantity" },
        ["Operations.WorkOrderMaterialUsage"] = new[] { "workOrderId", "stockDocumentLineId", "materialId", "usedQuantity" },
        ["Operations.WorkOrderStatusHistory"] = new[] { "workOrderId", "fromStatus", "toStatus", "changedAt", "note" },
        ["Operations.WorkOrderType"] = new[] { "code", "name", "isActive" },
        ["Organization.Employee"] = new[] { "companyId", "branchId", "departmentId", "employeePositionId", "userId", "code", "firstName", "lastName", "nationalId", "phone", "email", "hireDate", "terminationDate", "isActive" },
        ["Organization.EmployeePosition"] = new[] { "code", "name", "isActive" },
        ["Organization.EmployeeSkill"] = new[] { "code", "name", "isActive" },
        ["Organization.EmployeeSkillAssignment"] = new[] { "employeeId", "employeeSkillId", "level", "note" },
        ["Organization.ExpenseClaim"] = new[] { "employeeId", "projectId", "currencyId", "claimNo", "claimDate", "totalAmount", "status", "approvalRequestId" },
        ["Organization.ExpenseClaimLine"] = new[] { "expenseClaimId", "description", "expenseDate", "amount", "category" },
        ["Organization.LeaveRequest"] = new[] { "employeeId", "leaveType", "startDate", "endDate", "days", "reason", "status", "approvalRequestId" },
        ["Procurement.PurchaseOrder"] = new[] { "supplierId", "projectId", "status", "orderNo", "currencyId", "orderDate", "approvalRequestId" },
        ["Procurement.PurchaseOrderLine"] = new[] { "purchaseOrderId", "requestLineId", "materialId", "quantity", "unitPrice", "currencyId", "receivedQuantity" },
        ["Procurement.PurchaseReceipt"] = new[] { "supplierId", "purchaseOrderId", "warehouseId", "stockDocumentId", "receiptNo", "receiptDate", "status" },
        ["Procurement.PurchaseReceiptLine"] = new[] { "purchaseReceiptId", "purchaseOrderLineId", "materialId", "quantity", "unitPrice" },
        ["Procurement.SupplierInvoice"] = new[] { "supplierId", "purchaseOrderId", "purchaseReceiptId", "currencyId", "invoiceNo", "invoiceDate", "totalAmount", "status" },
        ["Procurement.SupplierInvoiceLine"] = new[] { "supplierInvoiceId", "materialId", "description", "quantity", "unitPrice", "taxRate" },
        ["Procurement.SupplierQuote"] = new[] { "supplierId", "projectId", "currencyId", "quoteNo", "quoteDate", "paymentTerm", "status", "isSelected" },
        ["Procurement.SupplierQuoteLine"] = new[] { "supplierQuoteId", "requestLineId", "materialId", "description", "quantity", "unitPrice", "taxRate", "discountRate", "deliveryDays" },
        ["ProgressPayments.ProgressPayment"] = new[] { "contractId", "partnerId", "progressPaymentNo", "paymentPeriodStart", "paymentPeriodEnd", "grossAmount", "deductionTotal", "netAmount", "status", "approvalRequestId" },
        ["ProgressPayments.ProgressPaymentDeduction"] = new[] { "progressPaymentId", "deductionType", "amount", "note" },
        ["ProgressPayments.ProgressPaymentLine"] = new[] { "progressPaymentId", "contractLineId", "measurementSheetLineId", "description", "quantity", "unitPrice", "amount" },
        ["Projects.Project"] = new[] { "companyId", "branchId", "projectTypeId", "statusId", "customerId", "managerUserId", "code", "name", "startDate", "endDate", "description" },
        ["Projects.ProjectLocation"] = new[] { "projectId", "parentLocationId", "code", "name" },
        ["Projects.ProjectMember"] = new[] { "projectId", "userId", "employeeId", "projectRole" },
        ["Projects.ProjectNote"] = new[] { "projectId", "title", "body" },
        ["Projects.ProjectPhas"] = new[] { "projectId", "parentPhaseId", "code", "name", "progressPercentage" },
        ["Projects.ProjectStatus"] = new[] { "code", "name", "displayOrder", "isClosedState", "isActive" },
        ["Projects.ProjectType"] = new[] { "code", "name", "isActive" },
        ["Reporting.DashboardWidget"] = new[] { "code", "name", "module", "widgetType", "requiredPermissionCode", "displayOrder", "isActive" },
        ["Reporting.ReportDefinition"] = new[] { "code", "name", "module", "queryKey", "requiredPermissionCode", "isActive" },
        ["Requests.Request"] = new[] { "requestTypeId", "projectId", "requestedByUserId", "status", "requestNo", "requestDate", "description", "approvalRequestId" },
        ["Requests.RequestLine"] = new[] { "requestId", "materialId", "requestedMaterialText", "quantity", "unitOfMeasureId", "note" },
        ["Requests.RequestType"] = new[] { "code", "name", "category", "isActive" },
        ["Workflow.ApprovalAction"] = new[] { "approvalRequestId", "approvalRequestStepId", "userId", "actionType", "actionAt", "note" },
        ["Workflow.ApprovalCondition"] = new[] { "approvalDefinitionVersionId", "fieldName", "operator", "valueText", "valueNumber" },
        ["Workflow.ApprovalDefinition"] = new[] { "code", "name", "relatedModule", "relatedEntityType", "isActive" },
        ["Workflow.ApprovalDefinitionVersion"] = new[] { "approvalDefinitionId", "versionNo", "effectiveFrom", "effectiveTo", "isActive" },
        ["Workflow.ApprovalDelegation"] = new[] { "delegatorUserId", "delegateUserId", "startDate", "endDate", "isActive" },
        ["Workflow.ApprovalRequest"] = new[] { "approvalDefinitionVersionId", "relatedModule", "relatedEntityType", "relatedEntityId", "requestedByUserId", "status", "currentStepNo" },
        ["Workflow.ApprovalRequestApprover"] = new[] { "approvalRequestStepId", "userId", "status", "actionAt", "delegatedFromUserId" },
        ["Workflow.ApprovalRequestStep"] = new[] { "approvalRequestId", "approvalStepDefinitionId", "stepNo", "status", "approvalMode", "requiredApprovalCount" },
        ["Workflow.ApprovalStepApprover"] = new[] { "approvalStepDefinitionId", "approverType", "approverUserId", "approverRoleId", "approverDepartmentId" },
        ["Workflow.ApprovalStepDefinition"] = new[] { "approvalDefinitionVersionId", "stepNo", "approvalMode", "requiredApprovalCount", "isRequired", "name" },
    };

    // Süreç (operasyonel) ekranları — kendi Process.* metin bölümüne sahiptir.
    public static readonly HashSet<string> ProcessScreens = new(System.StringComparer.OrdinalIgnoreCase)
    {
        "Procurement.Processes.GoodsReceipt",
        "Inventory.Processes.StockIssue",
        "Inventory.Processes.StockTransfer",
        "Workflow.Processes.Approval",
        "Finance.Processes.PaymentAllocation",
        "Finance.Processes.ProgressPaymentPosting",
        "Finance.Processes.TimesheetCost",
    };

    // Rapor ekranları — kendi Report.* metin bölümüne sahiptir.
    public static readonly HashSet<string> ReportScreens = new(System.StringComparer.OrdinalIgnoreCase)
    {
        "Projects.Reports.ProjectStatusReport",
        "ProgressPayments.Reports.ProgressPaymentSummary",
        "Procurement.Reports.PurchaseOrderSummary",
        "Inventory.Reports.StockBalanceReport",
        "HR.Reports.TimesheetSummary",
        "Finance.Reports.PayableAging",
        "Finance.Reports.ReceivableAging",
    };

    public static bool IsKnown(string? screenId)
        => !string.IsNullOrWhiteSpace(screenId)
           && (Registry.ContainsKey(screenId!)
               || ProcessScreens.Contains(screenId!)
               || ReportScreens.Contains(screenId!));

    // Verilen ekran için tüm metinleri içeren iç içe sözlük döndürür.
    // JSON'a çevrilip window.AppScreen olarak istemciye verilir.
    public static IDictionary<string, object?> Build(string screenId, IStringLocalizer localizer)
    {
        string Tx(string suffix)
        {
            var key = screenId + "." + suffix;
            var v = localizer[key];
            return v.ResourceNotFound ? string.Empty : v.Value;
        }

        var columns = new Dictionary<string, string>(System.StringComparer.OrdinalIgnoreCase);
        if (Registry.TryGetValue(screenId, out var fields))
        {
            foreach (var f in fields)
                columns[f] = Tx("Columns." + f);
        }

        var result = new Dictionary<string, object?>
        {
            ["id"] = screenId,
            ["title"] = Tx("Title"),
            ["description"] = Tx("Description"),
            ["columns"] = columns,
            ["actions"] = new Dictionary<string, string>
            {
                ["new"] = Tx("Actions.New"),
                ["edit"] = Tx("Actions.Edit"),
                ["delete"] = Tx("Actions.Delete"),
                ["save"] = Tx("Actions.Save"),
                ["cancel"] = Tx("Actions.Cancel"),
                ["export"] = Tx("Actions.Export"),
                ["refresh"] = Tx("Actions.Refresh"),
                ["columnChooser"] = Tx("Actions.ColumnChooser"),
            },
            ["grid"] = new Dictionary<string, string>
            {
                ["search"] = Tx("Grid.Search"),
                ["noData"] = Tx("Grid.NoData"),
                ["loading"] = Tx("Grid.Loading"),
            },
            ["notifications"] = new Dictionary<string, string>
            {
                ["saved"] = Tx("Notifications.Saved"),
                ["updated"] = Tx("Notifications.Updated"),
                ["deleted"] = Tx("Notifications.Deleted"),
                ["error"] = Tx("Notifications.Error"),
            },
            ["popup"] = new Dictionary<string, string>
            {
                ["createTitle"] = Tx("Popup.CreateTitle"),
                ["editTitle"] = Tx("Popup.EditTitle"),
            },
            ["validation"] = new Dictionary<string, string>
            {
                ["required"] = Tx("Validation.Required"),
            },
            ["confirm"] = new Dictionary<string, string>
            {
                ["delete"] = Tx("Confirm.Delete"),
            },
            // Yardım/açıklama paneli — her ekran kendi Help.* bölümünden.
            ["help"] = new Dictionary<string, string>
            {
                ["title"] = Tx("Help.Title"),
                ["button"] = Tx("Help.Button"),
                ["close"] = Tx("Help.Close"),
                ["purposeTitle"] = Tx("Help.PurposeTitle"),
                ["introGeneric"] = Tx("Help.IntroGeneric"),
                ["introEntity"] = Tx("Help.Intro"),
                ["stepsTitle"] = Tx("Help.StepsTitle"),
                ["gridTitle"] = Tx("Help.GridTitle"),
                ["gridSearch"] = Tx("Help.GridSearch"),
                ["gridFilterRow"] = Tx("Help.GridFilterRow"),
                ["gridHeaderFilter"] = Tx("Help.GridHeaderFilter"),
                ["gridColumnChooser"] = Tx("Help.GridColumnChooser"),
                ["gridSort"] = Tx("Help.GridSort"),
                ["gridExport"] = Tx("Help.GridExport"),
                ["gridPaging"] = Tx("Help.GridPaging"),
                ["actionsTitle"] = Tx("Help.ActionsTitle"),
                ["actionsExtraTitle"] = Tx("Help.ActionsExtraTitle"),
                ["actionAdd"] = Tx("Help.ActionAdd"),
                ["actionEdit"] = Tx("Help.ActionEdit"),
                ["actionDelete"] = Tx("Help.ActionDelete"),
                ["columnsTitle"] = Tx("Help.ColumnsTitle"),
                ["filtersTitle"] = Tx("Help.FiltersTitle"),
                ["relatedTitle"] = Tx("Help.RelatedTitle"),
                ["relatedNote"] = Tx("Help.RelatedNote"),
                ["lookupNote"] = Tx("Help.LookupNote"),
            },
            // Modül aksiyon butonları (onayla/reddet/iade vb.) — ekran-özel.
            ["moduleActions"] = new Dictionary<string, string>
            {
                ["column"] = Tx("ModuleActions.column"),
                ["approve"] = Tx("ModuleActions.approve"),
                ["reject"] = Tx("ModuleActions.reject"),
                ["return"] = Tx("ModuleActions.return"),
                ["cancel"] = Tx("ModuleActions.cancel"),
                ["reverse"] = Tx("ModuleActions.reverse"),
                ["receive"] = Tx("ModuleActions.receive"),
                ["close"] = Tx("ModuleActions.close"),
                ["reopen"] = Tx("ModuleActions.reopen"),
                ["activate"] = Tx("ModuleActions.activate"),
                ["validate"] = Tx("ModuleActions.validate"),
                ["confirmTitle"] = Tx("ModuleActions.confirmTitle"),
                ["confirmMessage"] = Tx("ModuleActions.confirmMessage"),
                ["notePrompt"] = Tx("ModuleActions.notePrompt"),
                ["succeeded"] = Tx("ModuleActions.succeeded"),
                ["validationOk"] = Tx("ModuleActions.validationOk"),
                ["validationIssues"] = Tx("ModuleActions.validationIssues"),
            },
            // Diyalog/uyarı butonları — ekran-özel.
            ["alerts"] = new Dictionary<string, string>
            {
                ["success"] = Tx("Alerts.success"),
                ["info"] = Tx("Alerts.info"),
                ["warning"] = Tx("Alerts.warning"),
                ["error"] = Tx("Alerts.error"),
                ["confirm"] = Tx("Alerts.confirm"),
                ["ok"] = Tx("Alerts.ok"),
                ["cancel"] = Tx("Alerts.cancel"),
            },
            // Genel diyalog metinleri — ekran-özel.
            ["common"] = new Dictionary<string, string>
            {
                ["error"] = Tx("Common.error"),
                ["cancel"] = Tx("Common.cancel"),
                ["save"] = Tx("Common.save"),
                ["yes"] = Tx("Common.yes"),
                ["no"] = Tx("Common.no"),
            },
            // Ekran tür rozeti (CRUD/süreç/rapor) — ekran-özel.
            ["screenChrome"] = new Dictionary<string, string>
            {
                ["entityBadge"] = Tx("ScreenChrome.entityBadge"),
                ["entityBadgeTitle"] = Tx("ScreenChrome.entityBadgeTitle"),
                ["processBadge"] = Tx("ScreenChrome.processBadge"),
                ["processBadgeTitle"] = Tx("ScreenChrome.processBadgeTitle"),
                ["reportBadge"] = Tx("ScreenChrome.reportBadge"),
                ["reportBadgeTitle"] = Tx("ScreenChrome.reportBadgeTitle"),
                ["lookupMissing"] = Tx("ScreenChrome.lookupMissing"),
            },
        };

        // Süreç ekranları: kendi Process.* bölümünden operasyonel metinler.
        if (ProcessScreens.Contains(screenId))
        {
            result["process"] = new Dictionary<string, string>
            {
                ["genericError"] = Tx("Process.GenericError"),
                ["genericSuccess"] = Tx("Process.GenericSuccess"),
                ["submit"] = Tx("Process.Submit"),
                ["reset"] = Tx("Process.Reset"),
                ["resultTotal"] = Tx("Process.ResultTotal"),
                ["resultTotalCost"] = Tx("Process.ResultTotalCost"),
                ["resultTransaction"] = Tx("Process.ResultTransaction"),
                ["resultLines"] = Tx("Process.ResultLines"),
                ["resultAllocations"] = Tx("Process.ResultAllocations"),
            };
        }

        // Rapor ekranları: kendi Report.* bölümünden filtre/aksiyon metinleri.
        if (ReportScreens.Contains(screenId))
        {
            result["report"] = new Dictionary<string, string>
            {
                ["startDate"] = Tx("Report.StartDate"),
                ["endDate"] = Tx("Report.EndDate"),
                ["status"] = Tx("Report.Status"),
                ["allStatuses"] = Tx("Report.AllStatuses"),
                ["export"] = Tx("Report.Export"),
                ["filters"] = Tx("Report.Filters"),
                ["refresh"] = Tx("Report.Refresh"),
            };
        }

        return result;
    }
}

using Energy.Web.Clients.Infrastructure.Authentication;
using Energy.Web.Clients.Infrastructure.ClientIdentity;
using Energy.Web.Configuration;
using Microsoft.Extensions.Options;

namespace Energy.Web.Clients;

/// <summary>Tüm per-entity API istemcilerinin (typed HttpClient) kaydı.</summary>
public static class EntityApiClientRegistration
{
    public static IServiceCollection AddEntityApiClients(this IServiceCollection services)
    {
        services.AddHttpClient<global::Energy.Web.Clients.Core.Company.ICompanyApiClient, global::Energy.Web.Clients.Core.Company.CompanyApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Core.Branch.IBranchApiClient, global::Energy.Web.Clients.Core.Branch.BranchApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Core.Department.IDepartmentApiClient, global::Energy.Web.Clients.Core.Department.DepartmentApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Core.Currency.ICurrencyApiClient, global::Energy.Web.Clients.Core.Currency.CurrencyApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Core.ExchangeRate.IExchangeRateApiClient, global::Energy.Web.Clients.Core.ExchangeRate.ExchangeRateApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Core.UnitOfMeasure.IUnitOfMeasureApiClient, global::Energy.Web.Clients.Core.UnitOfMeasure.UnitOfMeasureApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Core.UnitConversion.IUnitConversionApiClient, global::Energy.Web.Clients.Core.UnitConversion.UnitConversionApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Core.SequenceDefinition.ISequenceDefinitionApiClient, global::Energy.Web.Clients.Core.SequenceDefinition.SequenceDefinitionApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Core.SystemSetting.ISystemSettingApiClient, global::Energy.Web.Clients.Core.SystemSetting.SystemSettingApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Core.LocalizationResource.ILocalizationResourceApiClient, global::Energy.Web.Clients.Core.LocalizationResource.LocalizationResourceApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Core.AuditLog.IAuditLogApiClient, global::Energy.Web.Clients.Core.AuditLog.AuditLogApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Organization.Employee.IEmployeeApiClient, global::Energy.Web.Clients.Organization.Employee.EmployeeApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Organization.EmployeePosition.IEmployeePositionApiClient, global::Energy.Web.Clients.Organization.EmployeePosition.EmployeePositionApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Organization.EmployeeSkill.IEmployeeSkillApiClient, global::Energy.Web.Clients.Organization.EmployeeSkill.EmployeeSkillApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Organization.EmployeeSkillAssignment.IEmployeeSkillAssignmentApiClient, global::Energy.Web.Clients.Organization.EmployeeSkillAssignment.EmployeeSkillAssignmentApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Organization.LeaveRequest.ILeaveRequestApiClient, global::Energy.Web.Clients.Organization.LeaveRequest.LeaveRequestApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Organization.ExpenseClaim.IExpenseClaimApiClient, global::Energy.Web.Clients.Organization.ExpenseClaim.ExpenseClaimApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Organization.ExpenseClaimLine.IExpenseClaimLineApiClient, global::Energy.Web.Clients.Organization.ExpenseClaimLine.ExpenseClaimLineApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.BusinessPartners.BusinessPartner.IBusinessPartnerApiClient, global::Energy.Web.Clients.BusinessPartners.BusinessPartner.BusinessPartnerApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.BusinessPartners.BusinessPartnerContact.IBusinessPartnerContactApiClient, global::Energy.Web.Clients.BusinessPartners.BusinessPartnerContact.BusinessPartnerContactApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.BusinessPartners.BusinessPartnerAddress.IBusinessPartnerAddressApiClient, global::Energy.Web.Clients.BusinessPartners.BusinessPartnerAddress.BusinessPartnerAddressApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.BusinessPartners.BusinessPartnerBankAccount.IBusinessPartnerBankAccountApiClient, global::Energy.Web.Clients.BusinessPartners.BusinessPartnerBankAccount.BusinessPartnerBankAccountApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Projects.Project.IProjectApiClient, global::Energy.Web.Clients.Projects.Project.ProjectApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Projects.ProjectType.IProjectTypeApiClient, global::Energy.Web.Clients.Projects.ProjectType.ProjectTypeApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Projects.ProjectStatus.IProjectStatusApiClient, global::Energy.Web.Clients.Projects.ProjectStatus.ProjectStatusApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Projects.ProjectLocation.IProjectLocationApiClient, global::Energy.Web.Clients.Projects.ProjectLocation.ProjectLocationApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Projects.ProjectPhas.IProjectPhasApiClient, global::Energy.Web.Clients.Projects.ProjectPhas.ProjectPhasApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Projects.ProjectMember.IProjectMemberApiClient, global::Energy.Web.Clients.Projects.ProjectMember.ProjectMemberApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Projects.ProjectNote.IProjectNoteApiClient, global::Energy.Web.Clients.Projects.ProjectNote.ProjectNoteApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Catalog.Brand.IBrandApiClient, global::Energy.Web.Clients.Catalog.Brand.BrandApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Catalog.MaterialCategory.IMaterialCategoryApiClient, global::Energy.Web.Clients.Catalog.MaterialCategory.MaterialCategoryApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Catalog.MaterialAttributeDefinition.IMaterialAttributeDefinitionApiClient, global::Energy.Web.Clients.Catalog.MaterialAttributeDefinition.MaterialAttributeDefinitionApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Catalog.MaterialAttributeOption.IMaterialAttributeOptionApiClient, global::Energy.Web.Clients.Catalog.MaterialAttributeOption.MaterialAttributeOptionApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Catalog.MaterialCategoryAttribute.IMaterialCategoryAttributeApiClient, global::Energy.Web.Clients.Catalog.MaterialCategoryAttribute.MaterialCategoryAttributeApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Catalog.Material.IMaterialApiClient, global::Energy.Web.Clients.Catalog.Material.MaterialApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Catalog.MaterialAttributeValue.IMaterialAttributeValueApiClient, global::Energy.Web.Clients.Catalog.MaterialAttributeValue.MaterialAttributeValueApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Catalog.MaterialUnitConversion.IMaterialUnitConversionApiClient, global::Energy.Web.Clients.Catalog.MaterialUnitConversion.MaterialUnitConversionApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Inventory.Warehouse.IWarehouseApiClient, global::Energy.Web.Clients.Inventory.Warehouse.WarehouseApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Inventory.WarehouseLocation.IWarehouseLocationApiClient, global::Energy.Web.Clients.Inventory.WarehouseLocation.WarehouseLocationApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Inventory.StockDocumentType.IStockDocumentTypeApiClient, global::Energy.Web.Clients.Inventory.StockDocumentType.StockDocumentTypeApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Inventory.StockDocument.IStockDocumentApiClient, global::Energy.Web.Clients.Inventory.StockDocument.StockDocumentApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Inventory.StockDocumentLine.IStockDocumentLineApiClient, global::Energy.Web.Clients.Inventory.StockDocumentLine.StockDocumentLineApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Inventory.StockLot.IStockLotApiClient, global::Energy.Web.Clients.Inventory.StockLot.StockLotApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Inventory.StockIssueAllocation.IStockIssueAllocationApiClient, global::Energy.Web.Clients.Inventory.StockIssueAllocation.StockIssueAllocationApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Inventory.StockTransaction.IStockTransactionApiClient, global::Energy.Web.Clients.Inventory.StockTransaction.StockTransactionApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Inventory.StockBalance.IStockBalanceApiClient, global::Energy.Web.Clients.Inventory.StockBalance.StockBalanceApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Inventory.StockReservation.IStockReservationApiClient, global::Energy.Web.Clients.Inventory.StockReservation.StockReservationApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Inventory.StockCount.IStockCountApiClient, global::Energy.Web.Clients.Inventory.StockCount.StockCountApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Inventory.StockCountLine.IStockCountLineApiClient, global::Energy.Web.Clients.Inventory.StockCountLine.StockCountLineApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Inventory.WarehouseTransfer.IWarehouseTransferApiClient, global::Energy.Web.Clients.Inventory.WarehouseTransfer.WarehouseTransferApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Inventory.WarehouseTransferLine.IWarehouseTransferLineApiClient, global::Energy.Web.Clients.Inventory.WarehouseTransferLine.WarehouseTransferLineApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Requests.RequestType.IRequestTypeApiClient, global::Energy.Web.Clients.Requests.RequestType.RequestTypeApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Requests.Request.IRequestApiClient, global::Energy.Web.Clients.Requests.Request.RequestApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Requests.RequestLine.IRequestLineApiClient, global::Energy.Web.Clients.Requests.RequestLine.RequestLineApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Procurement.SupplierQuote.ISupplierQuoteApiClient, global::Energy.Web.Clients.Procurement.SupplierQuote.SupplierQuoteApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Procurement.SupplierQuoteLine.ISupplierQuoteLineApiClient, global::Energy.Web.Clients.Procurement.SupplierQuoteLine.SupplierQuoteLineApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Procurement.PurchaseOrder.IPurchaseOrderApiClient, global::Energy.Web.Clients.Procurement.PurchaseOrder.PurchaseOrderApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Procurement.PurchaseOrderLine.IPurchaseOrderLineApiClient, global::Energy.Web.Clients.Procurement.PurchaseOrderLine.PurchaseOrderLineApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Procurement.PurchaseReceipt.IPurchaseReceiptApiClient, global::Energy.Web.Clients.Procurement.PurchaseReceipt.PurchaseReceiptApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Procurement.PurchaseReceiptLine.IPurchaseReceiptLineApiClient, global::Energy.Web.Clients.Procurement.PurchaseReceiptLine.PurchaseReceiptLineApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Procurement.SupplierInvoice.ISupplierInvoiceApiClient, global::Energy.Web.Clients.Procurement.SupplierInvoice.SupplierInvoiceApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Procurement.SupplierInvoiceLine.ISupplierInvoiceLineApiClient, global::Energy.Web.Clients.Procurement.SupplierInvoiceLine.SupplierInvoiceLineApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Operations.WorkOrderType.IWorkOrderTypeApiClient, global::Energy.Web.Clients.Operations.WorkOrderType.WorkOrderTypeApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Operations.WorkOrder.IWorkOrderApiClient, global::Energy.Web.Clients.Operations.WorkOrder.WorkOrderApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Operations.WorkOrderAssignment.IWorkOrderAssignmentApiClient, global::Energy.Web.Clients.Operations.WorkOrderAssignment.WorkOrderAssignmentApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Operations.WorkOrderMaterialPlan.IWorkOrderMaterialPlanApiClient, global::Energy.Web.Clients.Operations.WorkOrderMaterialPlan.WorkOrderMaterialPlanApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Operations.WorkOrderMaterialUsage.IWorkOrderMaterialUsageApiClient, global::Energy.Web.Clients.Operations.WorkOrderMaterialUsage.WorkOrderMaterialUsageApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Operations.WorkOrderChecklist.IWorkOrderChecklistApiClient, global::Energy.Web.Clients.Operations.WorkOrderChecklist.WorkOrderChecklistApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Operations.WorkOrderChecklistItem.IWorkOrderChecklistItemApiClient, global::Energy.Web.Clients.Operations.WorkOrderChecklistItem.WorkOrderChecklistItemApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Operations.WorkOrderStatusHistory.IWorkOrderStatusHistoryApiClient, global::Energy.Web.Clients.Operations.WorkOrderStatusHistory.WorkOrderStatusHistoryApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.FieldOperations.DailySiteReport.IDailySiteReportApiClient, global::Energy.Web.Clients.FieldOperations.DailySiteReport.DailySiteReportApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.FieldOperations.DailySiteReportWorker.IDailySiteReportWorkerApiClient, global::Energy.Web.Clients.FieldOperations.DailySiteReportWorker.DailySiteReportWorkerApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.FieldOperations.DailySiteReportEquipment.IDailySiteReportEquipmentApiClient, global::Energy.Web.Clients.FieldOperations.DailySiteReportEquipment.DailySiteReportEquipmentApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.FieldOperations.DailySiteReportMaterial.IDailySiteReportMaterialApiClient, global::Energy.Web.Clients.FieldOperations.DailySiteReportMaterial.DailySiteReportMaterialApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.FieldOperations.ProgressEntry.IProgressEntryApiClient, global::Energy.Web.Clients.FieldOperations.ProgressEntry.ProgressEntryApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.FieldOperations.MeasurementSheet.IMeasurementSheetApiClient, global::Energy.Web.Clients.FieldOperations.MeasurementSheet.MeasurementSheetApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.FieldOperations.MeasurementSheetLine.IMeasurementSheetLineApiClient, global::Energy.Web.Clients.FieldOperations.MeasurementSheetLine.MeasurementSheetLineApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.HR.Timesheet.ITimesheetApiClient, global::Energy.Web.Clients.HR.Timesheet.TimesheetApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.HR.TimesheetLine.ITimesheetLineApiClient, global::Energy.Web.Clients.HR.TimesheetLine.TimesheetLineApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Assets.EquipmentAsset.IEquipmentAssetApiClient, global::Energy.Web.Clients.Assets.EquipmentAsset.EquipmentAssetApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Assets.EquipmentAssignment.IEquipmentAssignmentApiClient, global::Energy.Web.Clients.Assets.EquipmentAssignment.EquipmentAssignmentApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Assets.EquipmentMaintenance.IEquipmentMaintenanceApiClient, global::Energy.Web.Clients.Assets.EquipmentMaintenance.EquipmentMaintenanceApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Finance.FinancialAccount.IFinancialAccountApiClient, global::Energy.Web.Clients.Finance.FinancialAccount.FinancialAccountApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Finance.CostCenter.ICostCenterApiClient, global::Energy.Web.Clients.Finance.CostCenter.CostCenterApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Finance.FinancialTransaction.IFinancialTransactionApiClient, global::Energy.Web.Clients.Finance.FinancialTransaction.FinancialTransactionApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Finance.FinancialTransactionLine.IFinancialTransactionLineApiClient, global::Energy.Web.Clients.Finance.FinancialTransactionLine.FinancialTransactionLineApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Finance.Payable.IPayableApiClient, global::Energy.Web.Clients.Finance.Payable.PayableApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Finance.Receivable.IReceivableApiClient, global::Energy.Web.Clients.Finance.Receivable.ReceivableApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Finance.Payment.IPaymentApiClient, global::Energy.Web.Clients.Finance.Payment.PaymentApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Finance.PaymentAllocation.IPaymentAllocationApiClient, global::Energy.Web.Clients.Finance.PaymentAllocation.PaymentAllocationApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Finance.Collection.ICollectionApiClient, global::Energy.Web.Clients.Finance.Collection.CollectionApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Finance.CollectionAllocation.ICollectionAllocationApiClient, global::Energy.Web.Clients.Finance.CollectionAllocation.CollectionAllocationApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Budget.Budget.IBudgetApiClient, global::Energy.Web.Clients.Budget.Budget.BudgetApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Budget.BudgetLine.IBudgetLineApiClient, global::Energy.Web.Clients.Budget.BudgetLine.BudgetLineApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Contracts.Contract.IContractApiClient, global::Energy.Web.Clients.Contracts.Contract.ContractApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Contracts.ContractParty.IContractPartyApiClient, global::Energy.Web.Clients.Contracts.ContractParty.ContractPartyApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Contracts.ContractLine.IContractLineApiClient, global::Energy.Web.Clients.Contracts.ContractLine.ContractLineApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Contracts.ContractAmendment.IContractAmendmentApiClient, global::Energy.Web.Clients.Contracts.ContractAmendment.ContractAmendmentApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.ProgressPayments.ProgressPayment.IProgressPaymentApiClient, global::Energy.Web.Clients.ProgressPayments.ProgressPayment.ProgressPaymentApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.ProgressPayments.ProgressPaymentLine.IProgressPaymentLineApiClient, global::Energy.Web.Clients.ProgressPayments.ProgressPaymentLine.ProgressPaymentLineApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.ProgressPayments.ProgressPaymentDeduction.IProgressPaymentDeductionApiClient, global::Energy.Web.Clients.ProgressPayments.ProgressPaymentDeduction.ProgressPaymentDeductionApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Documents.DocumentFolder.IDocumentFolderApiClient, global::Energy.Web.Clients.Documents.DocumentFolder.DocumentFolderApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Documents.Document.IDocumentApiClient, global::Energy.Web.Clients.Documents.Document.DocumentApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Documents.DocumentVersion.IDocumentVersionApiClient, global::Energy.Web.Clients.Documents.DocumentVersion.DocumentVersionApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Documents.DocumentRelation.IDocumentRelationApiClient, global::Energy.Web.Clients.Documents.DocumentRelation.DocumentRelationApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Documents.DocumentPermission.IDocumentPermissionApiClient, global::Energy.Web.Clients.Documents.DocumentPermission.DocumentPermissionApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Workflow.ApprovalDefinition.IApprovalDefinitionApiClient, global::Energy.Web.Clients.Workflow.ApprovalDefinition.ApprovalDefinitionApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Workflow.ApprovalDefinitionVersion.IApprovalDefinitionVersionApiClient, global::Energy.Web.Clients.Workflow.ApprovalDefinitionVersion.ApprovalDefinitionVersionApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Workflow.ApprovalStepDefinition.IApprovalStepDefinitionApiClient, global::Energy.Web.Clients.Workflow.ApprovalStepDefinition.ApprovalStepDefinitionApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Workflow.ApprovalStepApprover.IApprovalStepApproverApiClient, global::Energy.Web.Clients.Workflow.ApprovalStepApprover.ApprovalStepApproverApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Workflow.ApprovalCondition.IApprovalConditionApiClient, global::Energy.Web.Clients.Workflow.ApprovalCondition.ApprovalConditionApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Workflow.ApprovalRequest.IApprovalRequestApiClient, global::Energy.Web.Clients.Workflow.ApprovalRequest.ApprovalRequestApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Workflow.ApprovalRequestStep.IApprovalRequestStepApiClient, global::Energy.Web.Clients.Workflow.ApprovalRequestStep.ApprovalRequestStepApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Workflow.ApprovalRequestApprover.IApprovalRequestApproverApiClient, global::Energy.Web.Clients.Workflow.ApprovalRequestApprover.ApprovalRequestApproverApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Workflow.ApprovalAction.IApprovalActionApiClient, global::Energy.Web.Clients.Workflow.ApprovalAction.ApprovalActionApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Workflow.ApprovalDelegation.IApprovalDelegationApiClient, global::Energy.Web.Clients.Workflow.ApprovalDelegation.ApprovalDelegationApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Notifications.Notification.INotificationApiClient, global::Energy.Web.Clients.Notifications.Notification.NotificationApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Notifications.NotificationRecipient.INotificationRecipientApiClient, global::Energy.Web.Clients.Notifications.NotificationRecipient.NotificationRecipientApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Notifications.NotificationPreference.INotificationPreferenceApiClient, global::Energy.Web.Clients.Notifications.NotificationPreference.NotificationPreferenceApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Reporting.ReportDefinition.IReportDefinitionApiClient, global::Energy.Web.Clients.Reporting.ReportDefinition.ReportDefinitionApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Reporting.DashboardWidget.IDashboardWidgetApiClient, global::Energy.Web.Clients.Reporting.DashboardWidget.DashboardWidgetApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        return services;
    }

    private static void Configure(IServiceProvider sp, HttpClient http)
    {
        var settings = sp.GetRequiredService<IOptions<ApiSettings>>().Value;
        if (string.IsNullOrWhiteSpace(settings.BaseUrl))
            throw new InvalidOperationException("Api:BaseUrl is not configured.");
        http.BaseAddress = new Uri(settings.BaseUrl);
    }
}

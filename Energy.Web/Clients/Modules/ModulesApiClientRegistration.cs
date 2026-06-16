using Energy.Web.Clients.Infrastructure.Authentication;
using Energy.Web.Clients.Infrastructure.ClientIdentity;
using Energy.Web.Configuration;
using Microsoft.Extensions.Options;

namespace Energy.Web.Clients.Modules;

/// <summary>Tüm per-entity Modules API istemcilerinin (typed HttpClient) kaydı.</summary>
public static class ModulesApiClientRegistration
{
    public static IServiceCollection AddModulesApiClients(this IServiceCollection services)
    {
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Core.Company.ICompanyApiClient, global::Energy.Web.Clients.Modules.Core.Company.CompanyApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Core.Branch.IBranchApiClient, global::Energy.Web.Clients.Modules.Core.Branch.BranchApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Core.Department.IDepartmentApiClient, global::Energy.Web.Clients.Modules.Core.Department.DepartmentApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Core.Currency.ICurrencyApiClient, global::Energy.Web.Clients.Modules.Core.Currency.CurrencyApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Core.ExchangeRate.IExchangeRateApiClient, global::Energy.Web.Clients.Modules.Core.ExchangeRate.ExchangeRateApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Core.UnitOfMeasure.IUnitOfMeasureApiClient, global::Energy.Web.Clients.Modules.Core.UnitOfMeasure.UnitOfMeasureApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Core.UnitConversion.IUnitConversionApiClient, global::Energy.Web.Clients.Modules.Core.UnitConversion.UnitConversionApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Core.SequenceDefinition.ISequenceDefinitionApiClient, global::Energy.Web.Clients.Modules.Core.SequenceDefinition.SequenceDefinitionApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Core.SystemSetting.ISystemSettingApiClient, global::Energy.Web.Clients.Modules.Core.SystemSetting.SystemSettingApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Core.LocalizationResource.ILocalizationResourceApiClient, global::Energy.Web.Clients.Modules.Core.LocalizationResource.LocalizationResourceApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Core.AuditLog.IAuditLogApiClient, global::Energy.Web.Clients.Modules.Core.AuditLog.AuditLogApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Organization.Employee.IEmployeeApiClient, global::Energy.Web.Clients.Modules.Organization.Employee.EmployeeApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Organization.EmployeePosition.IEmployeePositionApiClient, global::Energy.Web.Clients.Modules.Organization.EmployeePosition.EmployeePositionApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Organization.EmployeeSkill.IEmployeeSkillApiClient, global::Energy.Web.Clients.Modules.Organization.EmployeeSkill.EmployeeSkillApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Organization.EmployeeSkillAssignment.IEmployeeSkillAssignmentApiClient, global::Energy.Web.Clients.Modules.Organization.EmployeeSkillAssignment.EmployeeSkillAssignmentApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Organization.LeaveRequest.ILeaveRequestApiClient, global::Energy.Web.Clients.Modules.Organization.LeaveRequest.LeaveRequestApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Organization.ExpenseClaim.IExpenseClaimApiClient, global::Energy.Web.Clients.Modules.Organization.ExpenseClaim.ExpenseClaimApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Organization.ExpenseClaimLine.IExpenseClaimLineApiClient, global::Energy.Web.Clients.Modules.Organization.ExpenseClaimLine.ExpenseClaimLineApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.BusinessPartners.BusinessPartner.IBusinessPartnerApiClient, global::Energy.Web.Clients.Modules.BusinessPartners.BusinessPartner.BusinessPartnerApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.BusinessPartners.BusinessPartnerContact.IBusinessPartnerContactApiClient, global::Energy.Web.Clients.Modules.BusinessPartners.BusinessPartnerContact.BusinessPartnerContactApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.BusinessPartners.BusinessPartnerAddress.IBusinessPartnerAddressApiClient, global::Energy.Web.Clients.Modules.BusinessPartners.BusinessPartnerAddress.BusinessPartnerAddressApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.BusinessPartners.BusinessPartnerBankAccount.IBusinessPartnerBankAccountApiClient, global::Energy.Web.Clients.Modules.BusinessPartners.BusinessPartnerBankAccount.BusinessPartnerBankAccountApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Projects.Project.IProjectApiClient, global::Energy.Web.Clients.Modules.Projects.Project.ProjectApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Projects.ProjectType.IProjectTypeApiClient, global::Energy.Web.Clients.Modules.Projects.ProjectType.ProjectTypeApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Projects.ProjectStatus.IProjectStatusApiClient, global::Energy.Web.Clients.Modules.Projects.ProjectStatus.ProjectStatusApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Projects.ProjectLocation.IProjectLocationApiClient, global::Energy.Web.Clients.Modules.Projects.ProjectLocation.ProjectLocationApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Projects.ProjectPhas.IProjectPhasApiClient, global::Energy.Web.Clients.Modules.Projects.ProjectPhas.ProjectPhasApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Projects.ProjectMember.IProjectMemberApiClient, global::Energy.Web.Clients.Modules.Projects.ProjectMember.ProjectMemberApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Projects.ProjectNote.IProjectNoteApiClient, global::Energy.Web.Clients.Modules.Projects.ProjectNote.ProjectNoteApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Catalog.Brand.IBrandApiClient, global::Energy.Web.Clients.Modules.Catalog.Brand.BrandApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Catalog.MaterialCategory.IMaterialCategoryApiClient, global::Energy.Web.Clients.Modules.Catalog.MaterialCategory.MaterialCategoryApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Catalog.MaterialAttributeDefinition.IMaterialAttributeDefinitionApiClient, global::Energy.Web.Clients.Modules.Catalog.MaterialAttributeDefinition.MaterialAttributeDefinitionApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Catalog.MaterialAttributeOption.IMaterialAttributeOptionApiClient, global::Energy.Web.Clients.Modules.Catalog.MaterialAttributeOption.MaterialAttributeOptionApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Catalog.MaterialCategoryAttribute.IMaterialCategoryAttributeApiClient, global::Energy.Web.Clients.Modules.Catalog.MaterialCategoryAttribute.MaterialCategoryAttributeApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Catalog.Material.IMaterialApiClient, global::Energy.Web.Clients.Modules.Catalog.Material.MaterialApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Catalog.MaterialAttributeValue.IMaterialAttributeValueApiClient, global::Energy.Web.Clients.Modules.Catalog.MaterialAttributeValue.MaterialAttributeValueApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Catalog.MaterialUnitConversion.IMaterialUnitConversionApiClient, global::Energy.Web.Clients.Modules.Catalog.MaterialUnitConversion.MaterialUnitConversionApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Inventory.Warehouse.IWarehouseApiClient, global::Energy.Web.Clients.Modules.Inventory.Warehouse.WarehouseApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Inventory.WarehouseLocation.IWarehouseLocationApiClient, global::Energy.Web.Clients.Modules.Inventory.WarehouseLocation.WarehouseLocationApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Inventory.StockDocumentType.IStockDocumentTypeApiClient, global::Energy.Web.Clients.Modules.Inventory.StockDocumentType.StockDocumentTypeApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Inventory.StockDocument.IStockDocumentApiClient, global::Energy.Web.Clients.Modules.Inventory.StockDocument.StockDocumentApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Inventory.StockDocumentLine.IStockDocumentLineApiClient, global::Energy.Web.Clients.Modules.Inventory.StockDocumentLine.StockDocumentLineApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Inventory.StockLot.IStockLotApiClient, global::Energy.Web.Clients.Modules.Inventory.StockLot.StockLotApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Inventory.StockIssueAllocation.IStockIssueAllocationApiClient, global::Energy.Web.Clients.Modules.Inventory.StockIssueAllocation.StockIssueAllocationApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Inventory.StockTransaction.IStockTransactionApiClient, global::Energy.Web.Clients.Modules.Inventory.StockTransaction.StockTransactionApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Inventory.StockBalance.IStockBalanceApiClient, global::Energy.Web.Clients.Modules.Inventory.StockBalance.StockBalanceApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Inventory.StockReservation.IStockReservationApiClient, global::Energy.Web.Clients.Modules.Inventory.StockReservation.StockReservationApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Inventory.StockCount.IStockCountApiClient, global::Energy.Web.Clients.Modules.Inventory.StockCount.StockCountApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Inventory.StockCountLine.IStockCountLineApiClient, global::Energy.Web.Clients.Modules.Inventory.StockCountLine.StockCountLineApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Inventory.WarehouseTransfer.IWarehouseTransferApiClient, global::Energy.Web.Clients.Modules.Inventory.WarehouseTransfer.WarehouseTransferApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Inventory.WarehouseTransferLine.IWarehouseTransferLineApiClient, global::Energy.Web.Clients.Modules.Inventory.WarehouseTransferLine.WarehouseTransferLineApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Requests.RequestType.IRequestTypeApiClient, global::Energy.Web.Clients.Modules.Requests.RequestType.RequestTypeApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Requests.Request.IRequestApiClient, global::Energy.Web.Clients.Modules.Requests.Request.RequestApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Requests.RequestLine.IRequestLineApiClient, global::Energy.Web.Clients.Modules.Requests.RequestLine.RequestLineApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Procurement.SupplierQuote.ISupplierQuoteApiClient, global::Energy.Web.Clients.Modules.Procurement.SupplierQuote.SupplierQuoteApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Procurement.SupplierQuoteLine.ISupplierQuoteLineApiClient, global::Energy.Web.Clients.Modules.Procurement.SupplierQuoteLine.SupplierQuoteLineApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Procurement.PurchaseOrder.IPurchaseOrderApiClient, global::Energy.Web.Clients.Modules.Procurement.PurchaseOrder.PurchaseOrderApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Procurement.PurchaseOrderLine.IPurchaseOrderLineApiClient, global::Energy.Web.Clients.Modules.Procurement.PurchaseOrderLine.PurchaseOrderLineApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Procurement.PurchaseReceipt.IPurchaseReceiptApiClient, global::Energy.Web.Clients.Modules.Procurement.PurchaseReceipt.PurchaseReceiptApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Procurement.PurchaseReceiptLine.IPurchaseReceiptLineApiClient, global::Energy.Web.Clients.Modules.Procurement.PurchaseReceiptLine.PurchaseReceiptLineApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Procurement.SupplierInvoice.ISupplierInvoiceApiClient, global::Energy.Web.Clients.Modules.Procurement.SupplierInvoice.SupplierInvoiceApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Procurement.SupplierInvoiceLine.ISupplierInvoiceLineApiClient, global::Energy.Web.Clients.Modules.Procurement.SupplierInvoiceLine.SupplierInvoiceLineApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Operations.WorkOrderType.IWorkOrderTypeApiClient, global::Energy.Web.Clients.Modules.Operations.WorkOrderType.WorkOrderTypeApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Operations.WorkOrder.IWorkOrderApiClient, global::Energy.Web.Clients.Modules.Operations.WorkOrder.WorkOrderApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Operations.WorkOrderAssignment.IWorkOrderAssignmentApiClient, global::Energy.Web.Clients.Modules.Operations.WorkOrderAssignment.WorkOrderAssignmentApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Operations.WorkOrderMaterialPlan.IWorkOrderMaterialPlanApiClient, global::Energy.Web.Clients.Modules.Operations.WorkOrderMaterialPlan.WorkOrderMaterialPlanApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Operations.WorkOrderMaterialUsage.IWorkOrderMaterialUsageApiClient, global::Energy.Web.Clients.Modules.Operations.WorkOrderMaterialUsage.WorkOrderMaterialUsageApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Operations.WorkOrderChecklist.IWorkOrderChecklistApiClient, global::Energy.Web.Clients.Modules.Operations.WorkOrderChecklist.WorkOrderChecklistApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Operations.WorkOrderChecklistItem.IWorkOrderChecklistItemApiClient, global::Energy.Web.Clients.Modules.Operations.WorkOrderChecklistItem.WorkOrderChecklistItemApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Operations.WorkOrderStatusHistory.IWorkOrderStatusHistoryApiClient, global::Energy.Web.Clients.Modules.Operations.WorkOrderStatusHistory.WorkOrderStatusHistoryApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.FieldOperations.DailySiteReport.IDailySiteReportApiClient, global::Energy.Web.Clients.Modules.FieldOperations.DailySiteReport.DailySiteReportApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.FieldOperations.DailySiteReportWorker.IDailySiteReportWorkerApiClient, global::Energy.Web.Clients.Modules.FieldOperations.DailySiteReportWorker.DailySiteReportWorkerApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.FieldOperations.DailySiteReportEquipment.IDailySiteReportEquipmentApiClient, global::Energy.Web.Clients.Modules.FieldOperations.DailySiteReportEquipment.DailySiteReportEquipmentApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.FieldOperations.DailySiteReportMaterial.IDailySiteReportMaterialApiClient, global::Energy.Web.Clients.Modules.FieldOperations.DailySiteReportMaterial.DailySiteReportMaterialApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.FieldOperations.ProgressEntry.IProgressEntryApiClient, global::Energy.Web.Clients.Modules.FieldOperations.ProgressEntry.ProgressEntryApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.FieldOperations.MeasurementSheet.IMeasurementSheetApiClient, global::Energy.Web.Clients.Modules.FieldOperations.MeasurementSheet.MeasurementSheetApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.FieldOperations.MeasurementSheetLine.IMeasurementSheetLineApiClient, global::Energy.Web.Clients.Modules.FieldOperations.MeasurementSheetLine.MeasurementSheetLineApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.HR.Timesheet.ITimesheetApiClient, global::Energy.Web.Clients.Modules.HR.Timesheet.TimesheetApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.HR.TimesheetLine.ITimesheetLineApiClient, global::Energy.Web.Clients.Modules.HR.TimesheetLine.TimesheetLineApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Assets.EquipmentAsset.IEquipmentAssetApiClient, global::Energy.Web.Clients.Modules.Assets.EquipmentAsset.EquipmentAssetApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Assets.EquipmentAssignment.IEquipmentAssignmentApiClient, global::Energy.Web.Clients.Modules.Assets.EquipmentAssignment.EquipmentAssignmentApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Assets.EquipmentMaintenance.IEquipmentMaintenanceApiClient, global::Energy.Web.Clients.Modules.Assets.EquipmentMaintenance.EquipmentMaintenanceApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Finance.FinancialAccount.IFinancialAccountApiClient, global::Energy.Web.Clients.Modules.Finance.FinancialAccount.FinancialAccountApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Finance.CostCenter.ICostCenterApiClient, global::Energy.Web.Clients.Modules.Finance.CostCenter.CostCenterApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Finance.FinancialTransaction.IFinancialTransactionApiClient, global::Energy.Web.Clients.Modules.Finance.FinancialTransaction.FinancialTransactionApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Finance.FinancialTransactionLine.IFinancialTransactionLineApiClient, global::Energy.Web.Clients.Modules.Finance.FinancialTransactionLine.FinancialTransactionLineApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Finance.Payable.IPayableApiClient, global::Energy.Web.Clients.Modules.Finance.Payable.PayableApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Finance.Receivable.IReceivableApiClient, global::Energy.Web.Clients.Modules.Finance.Receivable.ReceivableApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Finance.Payment.IPaymentApiClient, global::Energy.Web.Clients.Modules.Finance.Payment.PaymentApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Finance.PaymentAllocation.IPaymentAllocationApiClient, global::Energy.Web.Clients.Modules.Finance.PaymentAllocation.PaymentAllocationApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Finance.Collection.ICollectionApiClient, global::Energy.Web.Clients.Modules.Finance.Collection.CollectionApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Finance.CollectionAllocation.ICollectionAllocationApiClient, global::Energy.Web.Clients.Modules.Finance.CollectionAllocation.CollectionAllocationApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Budget.Budget.IBudgetApiClient, global::Energy.Web.Clients.Modules.Budget.Budget.BudgetApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Budget.BudgetLine.IBudgetLineApiClient, global::Energy.Web.Clients.Modules.Budget.BudgetLine.BudgetLineApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Contracts.Contract.IContractApiClient, global::Energy.Web.Clients.Modules.Contracts.Contract.ContractApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Contracts.ContractParty.IContractPartyApiClient, global::Energy.Web.Clients.Modules.Contracts.ContractParty.ContractPartyApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Contracts.ContractLine.IContractLineApiClient, global::Energy.Web.Clients.Modules.Contracts.ContractLine.ContractLineApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Contracts.ContractAmendment.IContractAmendmentApiClient, global::Energy.Web.Clients.Modules.Contracts.ContractAmendment.ContractAmendmentApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.ProgressPayments.ProgressPayment.IProgressPaymentApiClient, global::Energy.Web.Clients.Modules.ProgressPayments.ProgressPayment.ProgressPaymentApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.ProgressPayments.ProgressPaymentLine.IProgressPaymentLineApiClient, global::Energy.Web.Clients.Modules.ProgressPayments.ProgressPaymentLine.ProgressPaymentLineApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.ProgressPayments.ProgressPaymentDeduction.IProgressPaymentDeductionApiClient, global::Energy.Web.Clients.Modules.ProgressPayments.ProgressPaymentDeduction.ProgressPaymentDeductionApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Documents.DocumentFolder.IDocumentFolderApiClient, global::Energy.Web.Clients.Modules.Documents.DocumentFolder.DocumentFolderApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Documents.Document.IDocumentApiClient, global::Energy.Web.Clients.Modules.Documents.Document.DocumentApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Documents.DocumentVersion.IDocumentVersionApiClient, global::Energy.Web.Clients.Modules.Documents.DocumentVersion.DocumentVersionApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Documents.DocumentRelation.IDocumentRelationApiClient, global::Energy.Web.Clients.Modules.Documents.DocumentRelation.DocumentRelationApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Documents.DocumentPermission.IDocumentPermissionApiClient, global::Energy.Web.Clients.Modules.Documents.DocumentPermission.DocumentPermissionApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Workflow.ApprovalDefinition.IApprovalDefinitionApiClient, global::Energy.Web.Clients.Modules.Workflow.ApprovalDefinition.ApprovalDefinitionApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Workflow.ApprovalDefinitionVersion.IApprovalDefinitionVersionApiClient, global::Energy.Web.Clients.Modules.Workflow.ApprovalDefinitionVersion.ApprovalDefinitionVersionApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Workflow.ApprovalStepDefinition.IApprovalStepDefinitionApiClient, global::Energy.Web.Clients.Modules.Workflow.ApprovalStepDefinition.ApprovalStepDefinitionApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Workflow.ApprovalStepApprover.IApprovalStepApproverApiClient, global::Energy.Web.Clients.Modules.Workflow.ApprovalStepApprover.ApprovalStepApproverApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Workflow.ApprovalCondition.IApprovalConditionApiClient, global::Energy.Web.Clients.Modules.Workflow.ApprovalCondition.ApprovalConditionApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Workflow.ApprovalRequest.IApprovalRequestApiClient, global::Energy.Web.Clients.Modules.Workflow.ApprovalRequest.ApprovalRequestApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Workflow.ApprovalRequestStep.IApprovalRequestStepApiClient, global::Energy.Web.Clients.Modules.Workflow.ApprovalRequestStep.ApprovalRequestStepApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Workflow.ApprovalRequestApprover.IApprovalRequestApproverApiClient, global::Energy.Web.Clients.Modules.Workflow.ApprovalRequestApprover.ApprovalRequestApproverApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Workflow.ApprovalAction.IApprovalActionApiClient, global::Energy.Web.Clients.Modules.Workflow.ApprovalAction.ApprovalActionApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Workflow.ApprovalDelegation.IApprovalDelegationApiClient, global::Energy.Web.Clients.Modules.Workflow.ApprovalDelegation.ApprovalDelegationApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Notifications.Notification.INotificationApiClient, global::Energy.Web.Clients.Modules.Notifications.Notification.NotificationApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Notifications.NotificationRecipient.INotificationRecipientApiClient, global::Energy.Web.Clients.Modules.Notifications.NotificationRecipient.NotificationRecipientApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Notifications.NotificationPreference.INotificationPreferenceApiClient, global::Energy.Web.Clients.Modules.Notifications.NotificationPreference.NotificationPreferenceApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Reporting.ReportDefinition.IReportDefinitionApiClient, global::Energy.Web.Clients.Modules.Reporting.ReportDefinition.ReportDefinitionApiClient>(Configure)
            .AddHttpMessageHandler<ClientIdentityHeaderHandler>()
            .AddHttpMessageHandler<AuthHeaderHandler>();
        services.AddHttpClient<global::Energy.Web.Clients.Modules.Reporting.DashboardWidget.IDashboardWidgetApiClient, global::Energy.Web.Clients.Modules.Reporting.DashboardWidget.DashboardWidgetApiClient>(Configure)
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

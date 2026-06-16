using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Energy.Domain.Modules.Assets;
using Energy.Domain.Modules.Budget;
using Energy.Domain.Modules.BusinessPartners;
using Energy.Domain.Modules.Catalog;
using Energy.Domain.Modules.Chat;
using Energy.Domain.Modules.Contracts;
using Energy.Domain.Modules.Core;
using Energy.Domain.Modules.Documents;
using Energy.Domain.Modules.FieldOperations;
using Energy.Domain.Modules.Finance;
using Energy.Domain.Modules.HR;
using Energy.Domain.Modules.IAM;
using Energy.Domain.Modules.Inventory;
using Energy.Domain.Modules.Notifications;
using Energy.Domain.Modules.Operations;
using Energy.Domain.Modules.Organization;
using Energy.Domain.Modules.Procurement;
using Energy.Domain.Modules.ProgressPayments;
using Energy.Domain.Modules.Projects;
using Energy.Domain.Modules.Reporting;
using Energy.Domain.Modules.Requests;
using Energy.Domain.Modules.Workflow;
using Energy.Domain.Common;
using Energy.Domain.Modules.IAM;

namespace Energy.Infrastructure.Persistence;

/// <summary>
/// Kanonik (Modules) EF Core bağlamı. 134 tablonun tamamını per-entity
/// yapılandırmalar + audit FK + soft-delete konvansiyonlarıyla eşleştirir.
/// </summary>
public class EnergyDbContext : DbContext
{
    public EnergyDbContext(DbContextOptions<EnergyDbContext> options) : base(options)
    {
    }

    public DbSet<global::Energy.Domain.Modules.Core.Company> Companies => Set<global::Energy.Domain.Modules.Core.Company>();
    public DbSet<global::Energy.Domain.Modules.Core.Branch> Branches => Set<global::Energy.Domain.Modules.Core.Branch>();
    public DbSet<global::Energy.Domain.Modules.Core.Department> Departments => Set<global::Energy.Domain.Modules.Core.Department>();
    public DbSet<global::Energy.Domain.Modules.Core.Currency> Currencies => Set<global::Energy.Domain.Modules.Core.Currency>();
    public DbSet<global::Energy.Domain.Modules.Core.ExchangeRate> ExchangeRates => Set<global::Energy.Domain.Modules.Core.ExchangeRate>();
    public DbSet<global::Energy.Domain.Modules.Core.UnitOfMeasure> UnitsOfMeasure => Set<global::Energy.Domain.Modules.Core.UnitOfMeasure>();
    public DbSet<global::Energy.Domain.Modules.Core.UnitConversion> UnitConversions => Set<global::Energy.Domain.Modules.Core.UnitConversion>();
    public DbSet<global::Energy.Domain.Modules.Core.SequenceDefinition> SequenceDefinitions => Set<global::Energy.Domain.Modules.Core.SequenceDefinition>();
    public DbSet<global::Energy.Domain.Modules.Core.SystemSetting> SystemSettings => Set<global::Energy.Domain.Modules.Core.SystemSetting>();
    public DbSet<global::Energy.Domain.Modules.Core.LocalizationResource> LocalizationResources => Set<global::Energy.Domain.Modules.Core.LocalizationResource>();
    public DbSet<global::Energy.Domain.Modules.Core.AuditLog> AuditLogs => Set<global::Energy.Domain.Modules.Core.AuditLog>();
    public DbSet<global::Energy.Domain.Modules.IAM.User> Users => Set<global::Energy.Domain.Modules.IAM.User>();
    public DbSet<global::Energy.Domain.Modules.IAM.Role> Roles => Set<global::Energy.Domain.Modules.IAM.Role>();
    public DbSet<global::Energy.Domain.Modules.IAM.Permission> Permissions => Set<global::Energy.Domain.Modules.IAM.Permission>();
    public DbSet<global::Energy.Domain.Modules.IAM.UserRole> UserRoles => Set<global::Energy.Domain.Modules.IAM.UserRole>();
    public DbSet<global::Energy.Domain.Modules.IAM.RolePermission> RolePermissions => Set<global::Energy.Domain.Modules.IAM.RolePermission>();
    public DbSet<global::Energy.Domain.Modules.IAM.UserPermission> UserPermissions => Set<global::Energy.Domain.Modules.IAM.UserPermission>();
    public DbSet<global::Energy.Domain.Modules.IAM.Menu> Menus => Set<global::Energy.Domain.Modules.IAM.Menu>();
    public DbSet<global::Energy.Domain.Modules.IAM.ApiEndpoint> ApiEndpoints => Set<global::Energy.Domain.Modules.IAM.ApiEndpoint>();
    public DbSet<global::Energy.Domain.Modules.IAM.UserSetting> UserSettings => Set<global::Energy.Domain.Modules.IAM.UserSetting>();
    public DbSet<global::Energy.Domain.Modules.Chat.ChatGroup> ChatGroups => Set<global::Energy.Domain.Modules.Chat.ChatGroup>();
    public DbSet<global::Energy.Domain.Modules.Chat.ChatGroupMember> ChatGroupMembers => Set<global::Energy.Domain.Modules.Chat.ChatGroupMember>();
    public DbSet<global::Energy.Domain.Modules.Chat.ChatMessage> ChatMessages => Set<global::Energy.Domain.Modules.Chat.ChatMessage>();
    public DbSet<global::Energy.Domain.Modules.Chat.ChatMessageReaction> ChatMessageReactions => Set<global::Energy.Domain.Modules.Chat.ChatMessageReaction>();
    public DbSet<global::Energy.Domain.Modules.Organization.Employee> Employees => Set<global::Energy.Domain.Modules.Organization.Employee>();
    public DbSet<global::Energy.Domain.Modules.Organization.EmployeePosition> EmployeePositions => Set<global::Energy.Domain.Modules.Organization.EmployeePosition>();
    public DbSet<global::Energy.Domain.Modules.Organization.EmployeeSkill> EmployeeSkills => Set<global::Energy.Domain.Modules.Organization.EmployeeSkill>();
    public DbSet<global::Energy.Domain.Modules.Organization.EmployeeSkillAssignment> EmployeeSkillAssignments => Set<global::Energy.Domain.Modules.Organization.EmployeeSkillAssignment>();
    public DbSet<global::Energy.Domain.Modules.Organization.LeaveRequest> LeaveRequests => Set<global::Energy.Domain.Modules.Organization.LeaveRequest>();
    public DbSet<global::Energy.Domain.Modules.Organization.ExpenseClaim> ExpenseClaims => Set<global::Energy.Domain.Modules.Organization.ExpenseClaim>();
    public DbSet<global::Energy.Domain.Modules.Organization.ExpenseClaimLine> ExpenseClaimLines => Set<global::Energy.Domain.Modules.Organization.ExpenseClaimLine>();
    public DbSet<global::Energy.Domain.Modules.BusinessPartners.BusinessPartner> BusinessPartners => Set<global::Energy.Domain.Modules.BusinessPartners.BusinessPartner>();
    public DbSet<global::Energy.Domain.Modules.BusinessPartners.BusinessPartnerContact> BusinessPartnerContacts => Set<global::Energy.Domain.Modules.BusinessPartners.BusinessPartnerContact>();
    public DbSet<global::Energy.Domain.Modules.BusinessPartners.BusinessPartnerAddress> BusinessPartnerAddresses => Set<global::Energy.Domain.Modules.BusinessPartners.BusinessPartnerAddress>();
    public DbSet<global::Energy.Domain.Modules.BusinessPartners.BusinessPartnerBankAccount> BusinessPartnerBankAccounts => Set<global::Energy.Domain.Modules.BusinessPartners.BusinessPartnerBankAccount>();
    public DbSet<global::Energy.Domain.Modules.Projects.Project> Projects => Set<global::Energy.Domain.Modules.Projects.Project>();
    public DbSet<global::Energy.Domain.Modules.Projects.ProjectType> ProjectTypes => Set<global::Energy.Domain.Modules.Projects.ProjectType>();
    public DbSet<global::Energy.Domain.Modules.Projects.ProjectStatus> ProjectStatuses => Set<global::Energy.Domain.Modules.Projects.ProjectStatus>();
    public DbSet<global::Energy.Domain.Modules.Projects.ProjectLocation> ProjectLocations => Set<global::Energy.Domain.Modules.Projects.ProjectLocation>();
    public DbSet<global::Energy.Domain.Modules.Projects.ProjectPhas> ProjectPhases => Set<global::Energy.Domain.Modules.Projects.ProjectPhas>();
    public DbSet<global::Energy.Domain.Modules.Projects.ProjectMember> ProjectMembers => Set<global::Energy.Domain.Modules.Projects.ProjectMember>();
    public DbSet<global::Energy.Domain.Modules.Projects.ProjectNote> ProjectNotes => Set<global::Energy.Domain.Modules.Projects.ProjectNote>();
    public DbSet<global::Energy.Domain.Modules.Catalog.Brand> Brands => Set<global::Energy.Domain.Modules.Catalog.Brand>();
    public DbSet<global::Energy.Domain.Modules.Catalog.MaterialCategory> MaterialCategories => Set<global::Energy.Domain.Modules.Catalog.MaterialCategory>();
    public DbSet<global::Energy.Domain.Modules.Catalog.MaterialAttributeDefinition> MaterialAttributeDefinitions => Set<global::Energy.Domain.Modules.Catalog.MaterialAttributeDefinition>();
    public DbSet<global::Energy.Domain.Modules.Catalog.MaterialAttributeOption> MaterialAttributeOptions => Set<global::Energy.Domain.Modules.Catalog.MaterialAttributeOption>();
    public DbSet<global::Energy.Domain.Modules.Catalog.MaterialCategoryAttribute> MaterialCategoryAttributes => Set<global::Energy.Domain.Modules.Catalog.MaterialCategoryAttribute>();
    public DbSet<global::Energy.Domain.Modules.Catalog.Material> Materials => Set<global::Energy.Domain.Modules.Catalog.Material>();
    public DbSet<global::Energy.Domain.Modules.Catalog.MaterialAttributeValue> MaterialAttributeValues => Set<global::Energy.Domain.Modules.Catalog.MaterialAttributeValue>();
    public DbSet<global::Energy.Domain.Modules.Catalog.MaterialUnitConversion> MaterialUnitConversions => Set<global::Energy.Domain.Modules.Catalog.MaterialUnitConversion>();
    public DbSet<global::Energy.Domain.Modules.Inventory.Warehouse> Warehouses => Set<global::Energy.Domain.Modules.Inventory.Warehouse>();
    public DbSet<global::Energy.Domain.Modules.Inventory.WarehouseLocation> WarehouseLocations => Set<global::Energy.Domain.Modules.Inventory.WarehouseLocation>();
    public DbSet<global::Energy.Domain.Modules.Inventory.StockDocumentType> StockDocumentTypes => Set<global::Energy.Domain.Modules.Inventory.StockDocumentType>();
    public DbSet<global::Energy.Domain.Modules.Inventory.StockDocument> StockDocuments => Set<global::Energy.Domain.Modules.Inventory.StockDocument>();
    public DbSet<global::Energy.Domain.Modules.Inventory.StockDocumentLine> StockDocumentLines => Set<global::Energy.Domain.Modules.Inventory.StockDocumentLine>();
    public DbSet<global::Energy.Domain.Modules.Inventory.StockLot> StockLots => Set<global::Energy.Domain.Modules.Inventory.StockLot>();
    public DbSet<global::Energy.Domain.Modules.Inventory.StockIssueAllocation> StockIssueAllocations => Set<global::Energy.Domain.Modules.Inventory.StockIssueAllocation>();
    public DbSet<global::Energy.Domain.Modules.Inventory.StockTransaction> StockTransactions => Set<global::Energy.Domain.Modules.Inventory.StockTransaction>();
    public DbSet<global::Energy.Domain.Modules.Inventory.StockBalance> StockBalances => Set<global::Energy.Domain.Modules.Inventory.StockBalance>();
    public DbSet<global::Energy.Domain.Modules.Inventory.StockReservation> StockReservations => Set<global::Energy.Domain.Modules.Inventory.StockReservation>();
    public DbSet<global::Energy.Domain.Modules.Inventory.StockCount> StockCounts => Set<global::Energy.Domain.Modules.Inventory.StockCount>();
    public DbSet<global::Energy.Domain.Modules.Inventory.StockCountLine> StockCountLines => Set<global::Energy.Domain.Modules.Inventory.StockCountLine>();
    public DbSet<global::Energy.Domain.Modules.Inventory.WarehouseTransfer> WarehouseTransfers => Set<global::Energy.Domain.Modules.Inventory.WarehouseTransfer>();
    public DbSet<global::Energy.Domain.Modules.Inventory.WarehouseTransferLine> WarehouseTransferLines => Set<global::Energy.Domain.Modules.Inventory.WarehouseTransferLine>();
    public DbSet<global::Energy.Domain.Modules.Requests.RequestType> RequestTypes => Set<global::Energy.Domain.Modules.Requests.RequestType>();
    public DbSet<global::Energy.Domain.Modules.Requests.Request> Requests => Set<global::Energy.Domain.Modules.Requests.Request>();
    public DbSet<global::Energy.Domain.Modules.Requests.RequestLine> RequestLines => Set<global::Energy.Domain.Modules.Requests.RequestLine>();
    public DbSet<global::Energy.Domain.Modules.Procurement.SupplierQuote> SupplierQuotes => Set<global::Energy.Domain.Modules.Procurement.SupplierQuote>();
    public DbSet<global::Energy.Domain.Modules.Procurement.SupplierQuoteLine> SupplierQuoteLines => Set<global::Energy.Domain.Modules.Procurement.SupplierQuoteLine>();
    public DbSet<global::Energy.Domain.Modules.Procurement.PurchaseOrder> PurchaseOrders => Set<global::Energy.Domain.Modules.Procurement.PurchaseOrder>();
    public DbSet<global::Energy.Domain.Modules.Procurement.PurchaseOrderLine> PurchaseOrderLines => Set<global::Energy.Domain.Modules.Procurement.PurchaseOrderLine>();
    public DbSet<global::Energy.Domain.Modules.Procurement.PurchaseReceipt> PurchaseReceipts => Set<global::Energy.Domain.Modules.Procurement.PurchaseReceipt>();
    public DbSet<global::Energy.Domain.Modules.Procurement.PurchaseReceiptLine> PurchaseReceiptLines => Set<global::Energy.Domain.Modules.Procurement.PurchaseReceiptLine>();
    public DbSet<global::Energy.Domain.Modules.Procurement.SupplierInvoice> SupplierInvoices => Set<global::Energy.Domain.Modules.Procurement.SupplierInvoice>();
    public DbSet<global::Energy.Domain.Modules.Procurement.SupplierInvoiceLine> SupplierInvoiceLines => Set<global::Energy.Domain.Modules.Procurement.SupplierInvoiceLine>();
    public DbSet<global::Energy.Domain.Modules.Operations.WorkOrderType> WorkOrderTypes => Set<global::Energy.Domain.Modules.Operations.WorkOrderType>();
    public DbSet<global::Energy.Domain.Modules.Operations.WorkOrder> WorkOrders => Set<global::Energy.Domain.Modules.Operations.WorkOrder>();
    public DbSet<global::Energy.Domain.Modules.Operations.WorkOrderAssignment> WorkOrderAssignments => Set<global::Energy.Domain.Modules.Operations.WorkOrderAssignment>();
    public DbSet<global::Energy.Domain.Modules.Operations.WorkOrderMaterialPlan> WorkOrderMaterialPlans => Set<global::Energy.Domain.Modules.Operations.WorkOrderMaterialPlan>();
    public DbSet<global::Energy.Domain.Modules.Operations.WorkOrderMaterialUsage> WorkOrderMaterialUsages => Set<global::Energy.Domain.Modules.Operations.WorkOrderMaterialUsage>();
    public DbSet<global::Energy.Domain.Modules.Operations.WorkOrderChecklist> WorkOrderChecklists => Set<global::Energy.Domain.Modules.Operations.WorkOrderChecklist>();
    public DbSet<global::Energy.Domain.Modules.Operations.WorkOrderChecklistItem> WorkOrderChecklistItems => Set<global::Energy.Domain.Modules.Operations.WorkOrderChecklistItem>();
    public DbSet<global::Energy.Domain.Modules.Operations.WorkOrderStatusHistory> WorkOrderStatusHistories => Set<global::Energy.Domain.Modules.Operations.WorkOrderStatusHistory>();
    public DbSet<global::Energy.Domain.Modules.FieldOperations.DailySiteReport> DailySiteReports => Set<global::Energy.Domain.Modules.FieldOperations.DailySiteReport>();
    public DbSet<global::Energy.Domain.Modules.FieldOperations.DailySiteReportWorker> DailySiteReportWorkers => Set<global::Energy.Domain.Modules.FieldOperations.DailySiteReportWorker>();
    public DbSet<global::Energy.Domain.Modules.FieldOperations.DailySiteReportEquipment> DailySiteReportEquipments => Set<global::Energy.Domain.Modules.FieldOperations.DailySiteReportEquipment>();
    public DbSet<global::Energy.Domain.Modules.FieldOperations.DailySiteReportMaterial> DailySiteReportMaterials => Set<global::Energy.Domain.Modules.FieldOperations.DailySiteReportMaterial>();
    public DbSet<global::Energy.Domain.Modules.FieldOperations.ProgressEntry> ProgressEntries => Set<global::Energy.Domain.Modules.FieldOperations.ProgressEntry>();
    public DbSet<global::Energy.Domain.Modules.FieldOperations.MeasurementSheet> MeasurementSheets => Set<global::Energy.Domain.Modules.FieldOperations.MeasurementSheet>();
    public DbSet<global::Energy.Domain.Modules.FieldOperations.MeasurementSheetLine> MeasurementSheetLines => Set<global::Energy.Domain.Modules.FieldOperations.MeasurementSheetLine>();
    public DbSet<global::Energy.Domain.Modules.HR.Timesheet> Timesheets => Set<global::Energy.Domain.Modules.HR.Timesheet>();
    public DbSet<global::Energy.Domain.Modules.HR.TimesheetLine> TimesheetLines => Set<global::Energy.Domain.Modules.HR.TimesheetLine>();
    public DbSet<global::Energy.Domain.Modules.Assets.EquipmentAsset> EquipmentAssets => Set<global::Energy.Domain.Modules.Assets.EquipmentAsset>();
    public DbSet<global::Energy.Domain.Modules.Assets.EquipmentAssignment> EquipmentAssignments => Set<global::Energy.Domain.Modules.Assets.EquipmentAssignment>();
    public DbSet<global::Energy.Domain.Modules.Assets.EquipmentMaintenance> EquipmentMaintenances => Set<global::Energy.Domain.Modules.Assets.EquipmentMaintenance>();
    public DbSet<global::Energy.Domain.Modules.Finance.FinancialAccount> FinancialAccounts => Set<global::Energy.Domain.Modules.Finance.FinancialAccount>();
    public DbSet<global::Energy.Domain.Modules.Finance.CostCenter> CostCenters => Set<global::Energy.Domain.Modules.Finance.CostCenter>();
    public DbSet<global::Energy.Domain.Modules.Finance.FinancialTransaction> FinancialTransactions => Set<global::Energy.Domain.Modules.Finance.FinancialTransaction>();
    public DbSet<global::Energy.Domain.Modules.Finance.FinancialTransactionLine> FinancialTransactionLines => Set<global::Energy.Domain.Modules.Finance.FinancialTransactionLine>();
    public DbSet<global::Energy.Domain.Modules.Finance.Payable> Payables => Set<global::Energy.Domain.Modules.Finance.Payable>();
    public DbSet<global::Energy.Domain.Modules.Finance.Receivable> Receivables => Set<global::Energy.Domain.Modules.Finance.Receivable>();
    public DbSet<global::Energy.Domain.Modules.Finance.Payment> Payments => Set<global::Energy.Domain.Modules.Finance.Payment>();
    public DbSet<global::Energy.Domain.Modules.Finance.PaymentAllocation> PaymentAllocations => Set<global::Energy.Domain.Modules.Finance.PaymentAllocation>();
    public DbSet<global::Energy.Domain.Modules.Finance.Collection> Collections => Set<global::Energy.Domain.Modules.Finance.Collection>();
    public DbSet<global::Energy.Domain.Modules.Finance.CollectionAllocation> CollectionAllocations => Set<global::Energy.Domain.Modules.Finance.CollectionAllocation>();
    public DbSet<global::Energy.Domain.Modules.Budget.Budget> Budgets => Set<global::Energy.Domain.Modules.Budget.Budget>();
    public DbSet<global::Energy.Domain.Modules.Budget.BudgetLine> BudgetLines => Set<global::Energy.Domain.Modules.Budget.BudgetLine>();
    public DbSet<global::Energy.Domain.Modules.Contracts.Contract> Contracts => Set<global::Energy.Domain.Modules.Contracts.Contract>();
    public DbSet<global::Energy.Domain.Modules.Contracts.ContractParty> ContractParties => Set<global::Energy.Domain.Modules.Contracts.ContractParty>();
    public DbSet<global::Energy.Domain.Modules.Contracts.ContractLine> ContractLines => Set<global::Energy.Domain.Modules.Contracts.ContractLine>();
    public DbSet<global::Energy.Domain.Modules.Contracts.ContractAmendment> ContractAmendments => Set<global::Energy.Domain.Modules.Contracts.ContractAmendment>();
    public DbSet<global::Energy.Domain.Modules.ProgressPayments.ProgressPayment> ProgressPayments => Set<global::Energy.Domain.Modules.ProgressPayments.ProgressPayment>();
    public DbSet<global::Energy.Domain.Modules.ProgressPayments.ProgressPaymentLine> ProgressPaymentLines => Set<global::Energy.Domain.Modules.ProgressPayments.ProgressPaymentLine>();
    public DbSet<global::Energy.Domain.Modules.ProgressPayments.ProgressPaymentDeduction> ProgressPaymentDeductions => Set<global::Energy.Domain.Modules.ProgressPayments.ProgressPaymentDeduction>();
    public DbSet<global::Energy.Domain.Modules.Documents.DocumentFolder> DocumentFolders => Set<global::Energy.Domain.Modules.Documents.DocumentFolder>();
    public DbSet<global::Energy.Domain.Modules.Documents.Document> Documents => Set<global::Energy.Domain.Modules.Documents.Document>();
    public DbSet<global::Energy.Domain.Modules.Documents.DocumentVersion> DocumentVersions => Set<global::Energy.Domain.Modules.Documents.DocumentVersion>();
    public DbSet<global::Energy.Domain.Modules.Documents.DocumentRelation> DocumentRelations => Set<global::Energy.Domain.Modules.Documents.DocumentRelation>();
    public DbSet<global::Energy.Domain.Modules.Documents.DocumentPermission> DocumentPermissions => Set<global::Energy.Domain.Modules.Documents.DocumentPermission>();
    public DbSet<global::Energy.Domain.Modules.Workflow.ApprovalDefinition> ApprovalDefinitions => Set<global::Energy.Domain.Modules.Workflow.ApprovalDefinition>();
    public DbSet<global::Energy.Domain.Modules.Workflow.ApprovalDefinitionVersion> ApprovalDefinitionVersions => Set<global::Energy.Domain.Modules.Workflow.ApprovalDefinitionVersion>();
    public DbSet<global::Energy.Domain.Modules.Workflow.ApprovalStepDefinition> ApprovalStepDefinitions => Set<global::Energy.Domain.Modules.Workflow.ApprovalStepDefinition>();
    public DbSet<global::Energy.Domain.Modules.Workflow.ApprovalStepApprover> ApprovalStepApprovers => Set<global::Energy.Domain.Modules.Workflow.ApprovalStepApprover>();
    public DbSet<global::Energy.Domain.Modules.Workflow.ApprovalCondition> ApprovalConditions => Set<global::Energy.Domain.Modules.Workflow.ApprovalCondition>();
    public DbSet<global::Energy.Domain.Modules.Workflow.ApprovalRequest> ApprovalRequests => Set<global::Energy.Domain.Modules.Workflow.ApprovalRequest>();
    public DbSet<global::Energy.Domain.Modules.Workflow.ApprovalRequestStep> ApprovalRequestSteps => Set<global::Energy.Domain.Modules.Workflow.ApprovalRequestStep>();
    public DbSet<global::Energy.Domain.Modules.Workflow.ApprovalRequestApprover> ApprovalRequestApprovers => Set<global::Energy.Domain.Modules.Workflow.ApprovalRequestApprover>();
    public DbSet<global::Energy.Domain.Modules.Workflow.ApprovalAction> ApprovalActions => Set<global::Energy.Domain.Modules.Workflow.ApprovalAction>();
    public DbSet<global::Energy.Domain.Modules.Workflow.ApprovalDelegation> ApprovalDelegations => Set<global::Energy.Domain.Modules.Workflow.ApprovalDelegation>();
    public DbSet<global::Energy.Domain.Modules.Notifications.Notification> Notifications => Set<global::Energy.Domain.Modules.Notifications.Notification>();
    public DbSet<global::Energy.Domain.Modules.Notifications.NotificationRecipient> NotificationRecipients => Set<global::Energy.Domain.Modules.Notifications.NotificationRecipient>();
    public DbSet<global::Energy.Domain.Modules.Notifications.NotificationPreference> NotificationPreferences => Set<global::Energy.Domain.Modules.Notifications.NotificationPreference>();
    public DbSet<global::Energy.Domain.Modules.Reporting.ReportDefinition> ReportDefinitions => Set<global::Energy.Domain.Modules.Reporting.ReportDefinition>();
    public DbSet<global::Energy.Domain.Modules.Reporting.DashboardWidget> DashboardWidgets => Set<global::Energy.Domain.Modules.Reporting.DashboardWidget>();

    protected override void ConfigureConventions(ModelConfigurationBuilder b)
    {
        b.Properties<decimal>().HavePrecision(18, 6);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(
            typeof(EnergyDbContext).Assembly,
            type => type.Namespace?.StartsWith(
                "Energy.Infrastructure.Persistence.Configurations.Modules",
                StringComparison.Ordinal) ?? false);

        ApplyAuditUserForeignKeys(builder);
        ApplySoftDeleteConvention(builder);
    }

    private static void ApplyAuditUserForeignKeys(ModelBuilder builder)
    {
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            if (!typeof(AuditableEntity).IsAssignableFrom(entityType.ClrType)) continue;
            typeof(EnergyDbContext)
                .GetMethod(nameof(ApplyAuditUserFk), BindingFlags.NonPublic | BindingFlags.Static)!
                .MakeGenericMethod(entityType.ClrType).Invoke(null, [builder]);
        }
    }

    private static void ApplyAuditUserFk<TEntity>(ModelBuilder builder) where TEntity : AuditableEntity
    {
        var entity = builder.Entity<TEntity>();
        entity.HasOne<User>().WithMany().HasForeignKey(e => e.CreatedBy).OnDelete(DeleteBehavior.Restrict);
        entity.HasOne<User>().WithMany().HasForeignKey(e => e.UpdatedBy).OnDelete(DeleteBehavior.Restrict);
        entity.HasOne<User>().WithMany().HasForeignKey(e => e.DeletedBy).OnDelete(DeleteBehavior.Restrict);
    }

    private static void ApplySoftDeleteConvention(ModelBuilder builder)
    {
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            if (!typeof(AuditableEntity).IsAssignableFrom(entityType.ClrType)) continue;
            if (entityType.GetDeclaredQueryFilters().Any()) continue;
            typeof(EnergyDbContext)
                .GetMethod(nameof(ApplySoftDeleteFilter), BindingFlags.NonPublic | BindingFlags.Static)!
                .MakeGenericMethod(entityType.ClrType).Invoke(null, [builder]);
        }
    }

    private static void ApplySoftDeleteFilter<TEntity>(ModelBuilder builder) where TEntity : AuditableEntity
        => builder.Entity<TEntity>().HasQueryFilter(e => !e.IsDeleted);
}

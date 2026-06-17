using BudgetEntity = Energy.Domain.Budget.Budget;
using Energy.Shared.Common;
using System.Reflection;
using Energy.Domain.Assets;
using Energy.Domain.BusinessPartners;
using Energy.Domain.Budget;
using Energy.Domain.Catalog;
using Energy.Domain.Chat;
using Energy.Domain.Common;
using Energy.Domain.Contracts;
using Energy.Domain.Core;
using Energy.Domain.Documents;
using Energy.Domain.FieldOperations;
using Energy.Domain.Finance;
using Energy.Domain.HR;
using Energy.Domain.IAM;
using Energy.Domain.Inventory;
using Energy.Domain.Notifications;
using Energy.Domain.Operations;
using Energy.Domain.Organization;
using Energy.Domain.Procurement;
using Energy.Domain.ProgressPayments;
using Energy.Domain.Projects;
using Energy.Domain.Reporting;
using Energy.Domain.Requests;
using Energy.Domain.Workflow;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // ---- IAM / System / Localization / Logger / Chat (mevcut) ----
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
    public DbSet<UserPermission> UserPermissions => Set<UserPermission>();
    public DbSet<UserSetting> UserSettings => Set<UserSetting>();
    public DbSet<Menu> Menus => Set<Menu>();
    public DbSet<ApiEndpoint> ApiEndpoints => Set<ApiEndpoint>();
    public DbSet<Resource> Resources => Set<Resource>();
    // LocalizationResources, Resource varlığının modül-standardı (Core) takma adıdır.
    public DbSet<Resource> LocalizationResources => Set<Resource>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
    public DbSet<ChatMessage> ChatMessages => Set<ChatMessage>();
    public DbSet<ChatGroup> ChatGroups => Set<ChatGroup>();
    public DbSet<ChatGroupMember> ChatGroupMembers => Set<ChatGroupMember>();
    public DbSet<ChatMessageReaction> ChatMessageReactions => Set<ChatMessageReaction>();

    // ---- Core ----
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<Branch> Branches => Set<Branch>();
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Currency> Currencies => Set<Currency>();
    public DbSet<ExchangeRate> ExchangeRates => Set<ExchangeRate>();
    public DbSet<UnitOfMeasure> UnitsOfMeasure => Set<UnitOfMeasure>();
    public DbSet<UnitConversion> UnitConversions => Set<UnitConversion>();
    public DbSet<SequenceDefinition> SequenceDefinitions => Set<SequenceDefinition>();
    public DbSet<SystemSetting> SystemSettings => Set<SystemSetting>();

    // ---- Organization ----
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<EmployeePosition> EmployeePositions => Set<EmployeePosition>();
    public DbSet<EmployeeSkill> EmployeeSkills => Set<EmployeeSkill>();
    public DbSet<EmployeeSkillAssignment> EmployeeSkillAssignments => Set<EmployeeSkillAssignment>();
    public DbSet<LeaveRequest> LeaveRequests => Set<LeaveRequest>();
    public DbSet<ExpenseClaim> ExpenseClaims => Set<ExpenseClaim>();
    public DbSet<ExpenseClaimLine> ExpenseClaimLines => Set<ExpenseClaimLine>();

    // ---- BusinessPartners ----
    public DbSet<BusinessPartner> BusinessPartners => Set<BusinessPartner>();
    public DbSet<BusinessPartnerContact> BusinessPartnerContacts => Set<BusinessPartnerContact>();
    public DbSet<BusinessPartnerAddress> BusinessPartnerAddresses => Set<BusinessPartnerAddress>();
    public DbSet<BusinessPartnerBankAccount> BusinessPartnerBankAccounts => Set<BusinessPartnerBankAccount>();

    // ---- Projects ----
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<ProjectType> ProjectTypes => Set<ProjectType>();
    public DbSet<ProjectStatus> ProjectStatuses => Set<ProjectStatus>();
    public DbSet<ProjectLocation> ProjectLocations => Set<ProjectLocation>();
    public DbSet<ProjectPhase> ProjectPhases => Set<ProjectPhase>();
    public DbSet<ProjectMember> ProjectMembers => Set<ProjectMember>();
    public DbSet<ProjectNote> ProjectNotes => Set<ProjectNote>();

    // ---- Catalog ----
    public DbSet<Brand> Brands => Set<Brand>();
    public DbSet<MaterialCategory> MaterialCategories => Set<MaterialCategory>();
    public DbSet<MaterialAttributeDefinition> MaterialAttributeDefinitions => Set<MaterialAttributeDefinition>();
    public DbSet<MaterialAttributeOption> MaterialAttributeOptions => Set<MaterialAttributeOption>();
    public DbSet<MaterialCategoryAttribute> MaterialCategoryAttributes => Set<MaterialCategoryAttribute>();
    public DbSet<Material> Materials => Set<Material>();
    public DbSet<MaterialAttributeValue> MaterialAttributeValues => Set<MaterialAttributeValue>();
    public DbSet<MaterialUnitConversion> MaterialUnitConversions => Set<MaterialUnitConversion>();

    // ---- Inventory ----
    public DbSet<Warehouse> Warehouses => Set<Warehouse>();
    public DbSet<WarehouseLocation> WarehouseLocations => Set<WarehouseLocation>();
    public DbSet<StockDocumentType> StockDocumentTypes => Set<StockDocumentType>();
    public DbSet<StockDocument> StockDocuments => Set<StockDocument>();
    public DbSet<StockDocumentLine> StockDocumentLines => Set<StockDocumentLine>();
    public DbSet<StockLot> StockLots => Set<StockLot>();
    public DbSet<StockIssueAllocation> StockIssueAllocations => Set<StockIssueAllocation>();
    public DbSet<StockTransaction> StockTransactions => Set<StockTransaction>();
    public DbSet<StockBalance> StockBalances => Set<StockBalance>();
    public DbSet<StockReservation> StockReservations => Set<StockReservation>();
    public DbSet<StockCount> StockCounts => Set<StockCount>();
    public DbSet<StockCountLine> StockCountLines => Set<StockCountLine>();
    public DbSet<WarehouseTransfer> WarehouseTransfers => Set<WarehouseTransfer>();
    public DbSet<WarehouseTransferLine> WarehouseTransferLines => Set<WarehouseTransferLine>();

    // ---- Requests ----
    public DbSet<RequestType> RequestTypes => Set<RequestType>();
    public DbSet<Request> Requests => Set<Request>();
    public DbSet<RequestLine> RequestLines => Set<RequestLine>();

    // ---- Procurement ----
    public DbSet<SupplierQuote> SupplierQuotes => Set<SupplierQuote>();
    public DbSet<SupplierQuoteLine> SupplierQuoteLines => Set<SupplierQuoteLine>();
    public DbSet<PurchaseOrder> PurchaseOrders => Set<PurchaseOrder>();
    public DbSet<PurchaseOrderLine> PurchaseOrderLines => Set<PurchaseOrderLine>();
    public DbSet<PurchaseReceipt> PurchaseReceipts => Set<PurchaseReceipt>();
    public DbSet<PurchaseReceiptLine> PurchaseReceiptLines => Set<PurchaseReceiptLine>();
    public DbSet<SupplierInvoice> SupplierInvoices => Set<SupplierInvoice>();
    public DbSet<SupplierInvoiceLine> SupplierInvoiceLines => Set<SupplierInvoiceLine>();

    // ---- Operations ----
    public DbSet<WorkOrderType> WorkOrderTypes => Set<WorkOrderType>();
    public DbSet<WorkOrder> WorkOrders => Set<WorkOrder>();
    public DbSet<WorkOrderAssignment> WorkOrderAssignments => Set<WorkOrderAssignment>();
    public DbSet<WorkOrderMaterialPlan> WorkOrderMaterialPlans => Set<WorkOrderMaterialPlan>();
    public DbSet<WorkOrderMaterialUsage> WorkOrderMaterialUsages => Set<WorkOrderMaterialUsage>();
    public DbSet<WorkOrderChecklist> WorkOrderChecklists => Set<WorkOrderChecklist>();
    public DbSet<WorkOrderChecklistItem> WorkOrderChecklistItems => Set<WorkOrderChecklistItem>();
    public DbSet<WorkOrderStatusHistory> WorkOrderStatusHistories => Set<WorkOrderStatusHistory>();

    // ---- FieldOperations ----
    public DbSet<DailySiteReport> DailySiteReports => Set<DailySiteReport>();
    public DbSet<DailySiteReportWorker> DailySiteReportWorkers => Set<DailySiteReportWorker>();
    public DbSet<DailySiteReportEquipment> DailySiteReportEquipments => Set<DailySiteReportEquipment>();
    public DbSet<DailySiteReportMaterial> DailySiteReportMaterials => Set<DailySiteReportMaterial>();
    public DbSet<ProgressEntry> ProgressEntries => Set<ProgressEntry>();
    public DbSet<MeasurementSheet> MeasurementSheets => Set<MeasurementSheet>();
    public DbSet<MeasurementSheetLine> MeasurementSheetLines => Set<MeasurementSheetLine>();

    // ---- HR ----
    public DbSet<Timesheet> Timesheets => Set<Timesheet>();
    public DbSet<TimesheetLine> TimesheetLines => Set<TimesheetLine>();

    // ---- Assets ----
    public DbSet<EquipmentAsset> EquipmentAssets => Set<EquipmentAsset>();
    public DbSet<EquipmentAssignment> EquipmentAssignments => Set<EquipmentAssignment>();
    public DbSet<EquipmentMaintenance> EquipmentMaintenances => Set<EquipmentMaintenance>();

    // ---- Finance ----
    public DbSet<FinancialAccount> FinancialAccounts => Set<FinancialAccount>();
    public DbSet<CostCenter> CostCenters => Set<CostCenter>();
    public DbSet<FinancialTransaction> FinancialTransactions => Set<FinancialTransaction>();
    public DbSet<FinancialTransactionLine> FinancialTransactionLines => Set<FinancialTransactionLine>();
    public DbSet<Payable> Payables => Set<Payable>();
    public DbSet<Receivable> Receivables => Set<Receivable>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<PaymentAllocation> PaymentAllocations => Set<PaymentAllocation>();
    public DbSet<Collection> Collections => Set<Collection>();
    public DbSet<CollectionAllocation> CollectionAllocations => Set<CollectionAllocation>();

    // ---- BudgetEntity ----
    public DbSet<BudgetEntity> Budgets => Set<BudgetEntity>();
    public DbSet<BudgetLine> BudgetLines => Set<BudgetLine>();

    // ---- Contracts ----
    public DbSet<Contract> Contracts => Set<Contract>();
    public DbSet<ContractParty> ContractParties => Set<ContractParty>();
    public DbSet<ContractLine> ContractLines => Set<ContractLine>();
    public DbSet<ContractAmendment> ContractAmendments => Set<ContractAmendment>();

    // ---- ProgressPayments ----
    public DbSet<ProgressPayment> ProgressPayments => Set<ProgressPayment>();
    public DbSet<ProgressPaymentLine> ProgressPaymentLines => Set<ProgressPaymentLine>();
    public DbSet<ProgressPaymentDeduction> ProgressPaymentDeductions => Set<ProgressPaymentDeduction>();

    // ---- Documents ----
    public DbSet<DocumentFolder> DocumentFolders => Set<DocumentFolder>();
    public DbSet<Document> Documents => Set<Document>();
    public DbSet<DocumentVersion> DocumentVersions => Set<DocumentVersion>();
    public DbSet<DocumentRelation> DocumentRelations => Set<DocumentRelation>();
    public DbSet<DocumentPermission> DocumentPermissions => Set<DocumentPermission>();

    // ---- Workflow ----
    public DbSet<ApprovalDefinition> ApprovalDefinitions => Set<ApprovalDefinition>();
    public DbSet<ApprovalDefinitionVersion> ApprovalDefinitionVersions => Set<ApprovalDefinitionVersion>();
    public DbSet<ApprovalStepDefinition> ApprovalStepDefinitions => Set<ApprovalStepDefinition>();
    public DbSet<ApprovalStepApprover> ApprovalStepApprovers => Set<ApprovalStepApprover>();
    public DbSet<ApprovalCondition> ApprovalConditions => Set<ApprovalCondition>();
    public DbSet<ApprovalRequest> ApprovalRequests => Set<ApprovalRequest>();
    public DbSet<ApprovalRequestStep> ApprovalRequestSteps => Set<ApprovalRequestStep>();
    public DbSet<ApprovalRequestApprover> ApprovalRequestApprovers => Set<ApprovalRequestApprover>();
    public DbSet<ApprovalAction> ApprovalActions => Set<ApprovalAction>();
    public DbSet<ApprovalDelegation> ApprovalDelegations => Set<ApprovalDelegation>();

    // ---- Notifications ----
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<NotificationRecipient> NotificationRecipients => Set<NotificationRecipient>();
    public DbSet<NotificationPreference> NotificationPreferences => Set<NotificationPreference>();

    // ---- Reporting ----
    public DbSet<ReportDefinition> ReportDefinitions => Set<ReportDefinition>();
    public DbSet<DashboardWidget> DashboardWidgets => Set<DashboardWidget>();

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<decimal>().HavePrecision(18, 6);

        configurationBuilder.Properties<ApprovalMode>().HaveConversion<string>().HaveMaxLength(30);
        configurationBuilder.Properties<ApproverType>().HaveConversion<string>().HaveMaxLength(30);
        configurationBuilder.Properties<ConditionOperator>().HaveConversion<string>().HaveMaxLength(30);
        configurationBuilder.Properties<ApprovalActionType>().HaveConversion<string>().HaveMaxLength(30);
        configurationBuilder.Properties<ApprovalRequestStatus>().HaveConversion<string>().HaveMaxLength(30);
        configurationBuilder.Properties<ApprovalStepStatus>().HaveConversion<string>().HaveMaxLength(30);
        configurationBuilder.Properties<ApprovalApproverStatus>().HaveConversion<string>().HaveMaxLength(30);
        configurationBuilder.Properties<DocumentStatus>().HaveConversion<string>().HaveMaxLength(30);
        configurationBuilder.Properties<RequestStatus>().HaveConversion<string>().HaveMaxLength(30);
        configurationBuilder.Properties<PurchaseOrderStatus>().HaveConversion<string>().HaveMaxLength(30);
        configurationBuilder.Properties<WorkOrderStatus>().HaveConversion<string>().HaveMaxLength(30);
        configurationBuilder.Properties<FinancialTransactionType>().HaveConversion<string>().HaveMaxLength(30);
        configurationBuilder.Properties<WarehouseType>().HaveConversion<string>().HaveMaxLength(30);
        configurationBuilder.Properties<PartnerType>().HaveConversion<string>().HaveMaxLength(30);
        configurationBuilder.Properties<ContractType>().HaveConversion<string>().HaveMaxLength(30);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // Her tablo için ayrı IEntityTypeConfiguration uygulanır (per-entity standardı).
        // Birleşik (combine) yapılandırma dosyaları kullanılmaz; tüm modül
        // yapılandırmaları assembly taraması ile otomatik uygulanır.
        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        ApplyAuditUserForeignKeys(builder);
        ApplySoftDeleteConvention(builder);
    }

    /// <summary>
    /// <see cref="AuditableEntity"/>'den türeyen her varlık için <c>CreatedBy</c>,
    /// <c>UpdatedBy</c> ve <c>DeletedBy</c> alanlarını <see cref="User"/>'a (Users.Id)
    /// nullable, N:1 ve <see cref="DeleteBehavior.Restrict"/> yabancı anahtar olarak
    /// bağlar. Audit kullanıcısı silinse bile geçmiş iş kaydı bozulmaz (Restrict).
    /// Tasarım dokümanı Relationship Catalogue audit FK kuralını uygular.
    /// </summary>
    private static void ApplyAuditUserForeignKeys(ModelBuilder builder)
    {
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            if (!typeof(AuditableEntity).IsAssignableFrom(entityType.ClrType))
            {
                continue;
            }

            var method = typeof(AppDbContext)
                .GetMethod(nameof(ApplyAuditUserFk), BindingFlags.NonPublic | BindingFlags.Static)!
                .MakeGenericMethod(entityType.ClrType);
            method.Invoke(null, [builder]);
        }
    }

    private static void ApplyAuditUserFk<TEntity>(ModelBuilder builder)
        where TEntity : AuditableEntity
    {
        var entity = builder.Entity<TEntity>();
        entity.HasOne<User>().WithMany().HasForeignKey(e => e.CreatedBy).OnDelete(DeleteBehavior.Restrict);
        entity.HasOne<User>().WithMany().HasForeignKey(e => e.UpdatedBy).OnDelete(DeleteBehavior.Restrict);
        entity.HasOne<User>().WithMany().HasForeignKey(e => e.DeletedBy).OnDelete(DeleteBehavior.Restrict);
    }

    /// <summary>
    /// <see cref="AuditableEntity"/>'den türeyen ve kendi yapılandırmasında bir
    /// sorgu filtresi tanımlamamış tüm varlıklara global soft-delete filtresi uygular.
    /// </summary>
    private static void ApplySoftDeleteConvention(ModelBuilder builder)
    {
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            if (!typeof(AuditableEntity).IsAssignableFrom(entityType.ClrType))
            {
                continue;
            }

            if (entityType.GetDeclaredQueryFilters().Any())
            {
                continue;
            }

            var method = typeof(AppDbContext)
                .GetMethod(nameof(ApplySoftDeleteFilter), BindingFlags.NonPublic | BindingFlags.Static)!
                .MakeGenericMethod(entityType.ClrType);
            method.Invoke(null, [builder]);
        }
    }

    private static void ApplySoftDeleteFilter<TEntity>(ModelBuilder builder)
        where TEntity : AuditableEntity
        => builder.Entity<TEntity>().HasQueryFilter(e => !e.IsDeleted);
}

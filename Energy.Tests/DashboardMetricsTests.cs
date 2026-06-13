using Energy.Application.Identity.Services;
using Energy.Domain.Budget;
using Energy.Domain.BusinessPartners;
using Energy.Domain.Catalog;
using Energy.Domain.Common;
using Energy.Domain.Core;
using Energy.Domain.Finance;
using Energy.Domain.Identity;
using Energy.Domain.Inventory;
using Energy.Domain.Operations;
using Energy.Domain.Procurement;
using Energy.Domain.Projects;
using Energy.Domain.Reporting;
using Energy.Domain.Workflow;
using Energy.Infrastructure.Home.Services;
using Energy.Infrastructure.Persistence;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Energy.Tests;

/// <summary>
/// Gösterge panosu canlı metrik testleri: <see cref="HomeService.GetEnterpriseMetricsAsync"/>
/// her widget kodunu gerçek veriden doğru hesaplar ve yalnızca çağıranın yetkili olduğu
/// widget'ları döndürür (yetki süzgeci).
/// </summary>
public sealed class DashboardMetricsTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly AppDbContext _db;

    private static readonly (string Code, string Module, string Perm)[] Widgets =
    [
        ("LowStock", "Inventory", "Inventory.ReadAll"),
        ("PendingApprovals", "Workflow", "Workflow.ReadAll"),
        ("BudgetOverrun", "Budget", "Budget.ReadAll"),
        ("OrderDelivery", "Procurement", "Procurement.ReadAll"),
        ("WorkOrderProgress", "Operations", "Operations.ReadAll"),
    ];

    public DashboardMetricsTests()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();
        var options = new DbContextOptionsBuilder<AppDbContext>().UseSqlite(_connection).Options;
        _db = new AppDbContext(options);
        _db.Database.EnsureCreated();

        SeedWidgets();
        SeedBusinessGraph();
        _db.SaveChanges();
    }

    private void SeedWidgets()
    {
        var order = 1;
        foreach (var (code, module, perm) in Widgets)
        {
            // DashboardWidget.RequiredPermissionCode, Permission.Code'a FK'dir; önce izin kaydını oluştur.
            var parts = perm.Split('.');
            _db.Permissions.Add(new Permission
            {
                Code = perm, Module = parts[0], Action = parts[^1],
                DisplayNameKey = $"Permissions.{perm}.Name",
            });

            _db.DashboardWidgets.Add(new DashboardWidget
            {
                Id = Guid.NewGuid(),
                Code = code,
                Name = $"DashboardWidgets.{code}.Name",
                Module = module,
                WidgetType = "Counter",
                RequiredPermissionCode = perm,
                DisplayOrder = order++,
                IsActive = true,
            });
        }
    }

    private void SeedBusinessGraph()
    {
        // ---- Referans/üst kayıtlar (FK hedefleri) ----
        var currency = new Currency { Id = Guid.NewGuid(), Code = "TRY", Name = "TRY", IsActive = true };
        var unit = new UnitOfMeasure { Id = Guid.NewGuid(), Code = "Piece", Name = "Adet", IsActive = true };
        var company = new Company { Id = Guid.NewGuid(), Code = "CO", Name = "Demo", BaseCurrencyId = currency.Id, IsActive = true };
        var projectType = new ProjectType { Id = Guid.NewGuid(), Code = "T", Name = "Tür", IsActive = true };
        var projectStatus = new ProjectStatus { Id = Guid.NewGuid(), Code = "A", Name = "Aktif", DisplayOrder = 1, IsActive = true };
        var project = new Project
        {
            Id = Guid.NewGuid(), CompanyId = company.Id, ProjectTypeId = projectType.Id, StatusId = projectStatus.Id,
            Code = "PRJ", Name = "Proje",
        };
        var category = new MaterialCategory { Id = Guid.NewGuid(), Code = "CAT", Name = "Kategori", IsActive = true };
        var material1 = new Material { Id = Guid.NewGuid(), MaterialCategoryId = category.Id, BaseUnitOfMeasureId = unit.Id, Code = "M1", Name = "Malzeme 1", IsActive = true };
        var material2 = new Material { Id = Guid.NewGuid(), MaterialCategoryId = category.Id, BaseUnitOfMeasureId = unit.Id, Code = "M2", Name = "Malzeme 2", IsActive = true };
        var warehouse = new Warehouse { Id = Guid.NewGuid(), CompanyId = company.Id, WarehouseType = WarehouseType.Central, Code = "WH", Name = "Depo", IsActive = true };
        var supplier = new BusinessPartner { Id = Guid.NewGuid(), PartnerType = PartnerType.Supplier, Code = "SUP", Name = "Tedarikçi", IsActive = true };
        var workOrderType = new WorkOrderType { Id = Guid.NewGuid(), Code = "WOT", Name = "Tür", IsActive = true };
        var approvalDefinition = new ApprovalDefinition
        {
            Id = Guid.NewGuid(), Code = "DEF", Name = "Akış", RelatedModule = "Procurement", RelatedEntityType = "PurchaseOrder", IsActive = true,
        };
        var approvalVersion = new ApprovalDefinitionVersion
        {
            Id = Guid.NewGuid(), ApprovalDefinitionId = approvalDefinition.Id, VersionNo = 1, EffectiveFrom = DateTime.UtcNow, IsActive = true,
        };
        var requester = new User
        {
            Id = Guid.NewGuid(), UserName = "requester", Email = "requester@test.local",
            FirstName = "Talep", LastName = "Eden", PasswordHash = "x", IsActive = true, SecurityStamp = Guid.NewGuid(),
        };

        _db.Currencies.Add(currency);
        _db.UnitsOfMeasure.Add(unit);
        _db.Companies.Add(company);
        _db.ProjectTypes.Add(projectType);
        _db.ProjectStatuses.Add(projectStatus);
        _db.Projects.Add(project);
        _db.MaterialCategories.Add(category);
        _db.Materials.AddRange(material1, material2);
        _db.Warehouses.Add(warehouse);
        _db.BusinessPartners.Add(supplier);
        _db.WorkOrderTypes.Add(workOrderType);
        _db.ApprovalDefinitions.Add(approvalDefinition);
        _db.ApprovalDefinitionVersions.Add(approvalVersion);
        _db.Users.Add(requester);

        // LowStock = kullanılabilir (Quantity - Reserved) <= 0 olan bakiye sayısı → 1.
        _db.StockBalances.Add(new StockBalance
        {
            Id = Guid.NewGuid(), WarehouseId = warehouse.Id, MaterialId = material1.Id,
            Quantity = 0m, ReservedQuantity = 0m, TotalCost = 0m, LastRecalculatedAt = DateTime.UtcNow,
        });
        _db.StockBalances.Add(new StockBalance
        {
            Id = Guid.NewGuid(), WarehouseId = warehouse.Id, MaterialId = material2.Id,
            Quantity = 100m, ReservedQuantity = 0m, TotalCost = 5000m, LastRecalculatedAt = DateTime.UtcNow,
        });

        // PendingApprovals = Pending durumundaki onay talebi sayısı → 1 (Approved hariç).
        _db.ApprovalRequests.Add(new ApprovalRequest
        {
            Id = Guid.NewGuid(), ApprovalDefinitionVersionId = approvalVersion.Id,
            RelatedModule = "Procurement", RelatedEntityType = "PurchaseOrder", RelatedEntityId = Guid.NewGuid(),
            RequestedByUserId = requester.Id, Status = ApprovalRequestStatus.Pending, CurrentStepNo = 1,
        });
        _db.ApprovalRequests.Add(new ApprovalRequest
        {
            Id = Guid.NewGuid(), ApprovalDefinitionVersionId = approvalVersion.Id,
            RelatedModule = "Procurement", RelatedEntityType = "PurchaseOrder", RelatedEntityId = Guid.NewGuid(),
            RequestedByUserId = requester.Id, Status = ApprovalRequestStatus.Approved, CurrentStepNo = 2,
        });

        // BudgetOverrun = gerçekleşen > planlanan olan etkin bütçe sayısı → 1.
        var budget = new Budget
        {
            Id = Guid.NewGuid(), ProjectId = project.Id, CurrencyId = currency.Id,
            Name = "Test Bütçe", Year = DateTime.UtcNow.Year, IsActive = true,
        };
        _db.Budgets.Add(budget);
        _db.BudgetLines.Add(new BudgetLine
        {
            Id = Guid.NewGuid(), BudgetId = budget.Id, ProjectId = project.Id,
            Description = "Plan", PlannedAmount = 100m,
        });
        var tx = new FinancialTransaction
        {
            Id = Guid.NewGuid(), TransactionType = FinancialTransactionType.Expense, ProjectId = project.Id,
            CurrencyId = currency.Id, Amount = 150m, TransactionDate = DateTime.UtcNow,
        };
        _db.FinancialTransactions.Add(tx);
        _db.FinancialTransactionLines.Add(new FinancialTransactionLine
        {
            Id = Guid.NewGuid(), FinancialTransactionId = tx.Id, ProjectId = project.Id, Amount = 150m,
        });

        // OrderDelivery = onaylı/kısmen teslim sipariş sayısı → 1 (Received hariç).
        _db.PurchaseOrders.Add(new PurchaseOrder
        {
            Id = Guid.NewGuid(), SupplierId = supplier.Id, CurrencyId = currency.Id,
            Status = PurchaseOrderStatus.Approved, OrderNo = "PO-T1", OrderDate = DateTime.UtcNow,
        });
        _db.PurchaseOrders.Add(new PurchaseOrder
        {
            Id = Guid.NewGuid(), SupplierId = supplier.Id, CurrencyId = currency.Id,
            Status = PurchaseOrderStatus.Received, OrderNo = "PO-T2", OrderDate = DateTime.UtcNow,
        });

        // WorkOrderProgress = açık (tamamlanmamış/kapanmamış) iş emri sayısı → 1.
        _db.WorkOrders.Add(new WorkOrder
        {
            Id = Guid.NewGuid(), WorkOrderTypeId = workOrderType.Id, ProjectId = project.Id, Status = WorkOrderStatus.InProgress,
            WorkOrderNo = "WO-T1", Title = "Açık iş",
        });
        _db.WorkOrders.Add(new WorkOrder
        {
            Id = Guid.NewGuid(), WorkOrderTypeId = workOrderType.Id, ProjectId = project.Id, Status = WorkOrderStatus.Closed,
            WorkOrderNo = "WO-T2", Title = "Kapalı iş",
        });
    }

    private HomeService BuildService(params string[] permissions)
    {
        var user = new FakeCurrentUser(Guid.NewGuid());
        var resolver = new FakePermissionResolver(permissions.ToHashSet(StringComparer.OrdinalIgnoreCase));
        return new HomeService(_db, user, resolver);
    }

    [Fact]
    public async Task All_metrics_compute_expected_values_when_user_has_all_permissions()
    {
        var service = BuildService("Inventory.ReadAll", "Workflow.ReadAll", "Budget.ReadAll", "Procurement.ReadAll", "Operations.ReadAll");

        var metrics = await service.GetEnterpriseMetricsAsync();
        var byCode = metrics.ToDictionary(m => m.Code, m => m.Value);

        Assert.Equal(5, metrics.Count);
        Assert.Equal(1m, byCode["LowStock"]);
        Assert.Equal(1m, byCode["PendingApprovals"]);
        Assert.Equal(1m, byCode["BudgetOverrun"]);
        Assert.Equal(1m, byCode["OrderDelivery"]);
        Assert.Equal(1m, byCode["WorkOrderProgress"]);
    }

    [Fact]
    public async Task Metrics_are_filtered_by_caller_permissions()
    {
        var service = BuildService("Inventory.ReadAll");

        var metrics = await service.GetEnterpriseMetricsAsync();

        Assert.Single(metrics);
        Assert.Equal("LowStock", metrics[0].Code);
    }

    [Fact]
    public async Task No_metrics_returned_when_user_lacks_all_widget_permissions()
    {
        var service = BuildService();

        var metrics = await service.GetEnterpriseMetricsAsync();

        Assert.Empty(metrics);
    }

    [Fact]
    public async Task Metrics_are_ordered_by_display_order()
    {
        var service = BuildService("Inventory.ReadAll", "Workflow.ReadAll", "Budget.ReadAll", "Procurement.ReadAll", "Operations.ReadAll");

        var metrics = await service.GetEnterpriseMetricsAsync();

        var orders = metrics.Select(m => m.DisplayOrder).ToList();
        Assert.Equal(orders.OrderBy(o => o).ToList(), orders);
    }

    public void Dispose()
    {
        _db.Dispose();
        _connection.Dispose();
    }

    private sealed class FakeCurrentUser : ICurrentUser
    {
        public FakeCurrentUser(Guid? id) => UserId = id;
        public Guid? UserId { get; }
        public string? UserName => "test";
        public bool IsAuthenticated => UserId is not null;
    }

    private sealed class FakePermissionResolver : IPermissionResolver
    {
        private readonly IReadOnlySet<string> _permissions;
        public FakePermissionResolver(IReadOnlySet<string> permissions) => _permissions = permissions;
        public Task<IReadOnlySet<string>> GetPermissionsAsync(Guid userId, CancellationToken ct = default)
            => Task.FromResult(_permissions);
        public Task<bool> HasPermissionAsync(Guid userId, string permissionCode, CancellationToken ct = default)
            => Task.FromResult(_permissions.Contains(permissionCode));
        public void InvalidateUser(Guid userId) { }
        public Task InvalidateRoleAsync(Guid roleId, CancellationToken ct = default) => Task.CompletedTask;
    }
}


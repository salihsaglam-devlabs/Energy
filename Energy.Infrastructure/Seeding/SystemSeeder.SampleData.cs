using System.Linq.Expressions;
using Energy.Domain.Common;
using Energy.Domain.Core;
using Energy.Domain.Projects;
using Energy.Domain.BusinessPartners;
using Energy.Domain.Catalog;
using Energy.Domain.Inventory;
using Energy.Domain.Operations;
using Energy.Domain.Procurement;
using Energy.Domain.Budget;
using Energy.Domain.Finance;
using Energy.Domain.Workflow;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Energy.Infrastructure.Seeding;

/// <summary>
/// Demo iş verisinin (başlık + satır) idempotent tohumlaması. Tasarım dokümanındaki
/// uçtan uca akışı temsil eden küçük ama tutarlı bir veri grafiği oluşturur: şirket →
/// proje → cari → malzeme/depo → stok → iş emri → satın alma → bütçe/finans → onay.
/// Amaç, gösterge panosu metriklerinin (LowStock, PendingApprovals, BudgetOverrun,
/// OrderDelivery, WorkOrderProgress) ilk kurulumda canlı/dolu görünmesi ve modül
/// ekranlarının boş açılmamasıdır. Her kayıt doğal anahtarına göre korunur; yeniden
/// çalıştırma kopya üretmez.
/// </summary>
public sealed partial class SystemSeeder
{
    private const string DemoMarker = "SEED-DEMO";

    private async Task EnsureSampleBusinessDataAsync(CancellationToken ct)
    {
        // Referans veriler (para birimi, ölçü birimi) önce tohumlanmış olmalı.
        var currency = await _db.Currencies.IgnoreQueryFilters().FirstOrDefaultAsync(c => c.Code == "TRY", ct);
        var unit = await _db.UnitsOfMeasure.IgnoreQueryFilters().FirstOrDefaultAsync(u => u.Code == "Piece", ct);
        if (currency is null || unit is null)
        {
            _logger.LogWarning("Sample business data skipped: reference currency/unit not found.");
            return;
        }

        // 1) Şirket
        var company = await GetOrAddAsync(_db.Companies, c => c.Code == "DEMO-CO", () => new Company
        {
            Id = Guid.NewGuid(), Code = "DEMO-CO", Name = "Demo İnşaat A.Ş.",
            BaseCurrencyId = currency.Id, IsActive = true,
        }, ct);

        // 2) Proje türü + durumu + proje
        var projectType = await GetOrAddAsync(_db.ProjectTypes, t => t.Code == "CONSTR", () => new ProjectType
        {
            Id = Guid.NewGuid(), Code = "CONSTR", Name = "İnşaat", IsActive = true,
        }, ct);
        var projectStatus = await GetOrAddAsync(_db.ProjectStatuses, s => s.Code == "ACTIVE", () => new ProjectStatus
        {
            Id = Guid.NewGuid(), Code = "ACTIVE", Name = "Aktif", DisplayOrder = 1, IsClosedState = false, IsActive = true,
        }, ct);
        var project = await GetOrAddAsync(_db.Projects, p => p.Code == "PRJ-001", () => new Project
        {
            Id = Guid.NewGuid(), CompanyId = company.Id, ProjectTypeId = projectType.Id, StatusId = projectStatus.Id,
            Code = "PRJ-001", Name = "Merkez Saha Projesi", StartDate = DateTime.UtcNow.AddMonths(-2),
        }, ct);

        // 3) Tedarikçi (cari)
        var supplier = await GetOrAddAsync(_db.BusinessPartners, b => b.Code == "SUP-001", () => new BusinessPartner
        {
            Id = Guid.NewGuid(), PartnerType = PartnerType.Supplier, Code = "SUP-001",
            Name = "Anadolu Malzeme Ltd.", IsActive = true,
        }, ct);

        // 4) Malzeme kategorisi + iki malzeme
        var category = await GetOrAddAsync(_db.MaterialCategories, c => c.Code == "CAT-001", () => new MaterialCategory
        {
            Id = Guid.NewGuid(), Code = "CAT-001", Name = "Genel Malzeme", IsActive = true,
        }, ct);
        var material1 = await GetOrAddAsync(_db.Materials, m => m.Code == "MAT-001", () => new Material
        {
            Id = Guid.NewGuid(), MaterialCategoryId = category.Id, BaseUnitOfMeasureId = unit.Id,
            Code = "MAT-001", Name = "Çimento 50kg", IsActive = true,
        }, ct);
        var material2 = await GetOrAddAsync(_db.Materials, m => m.Code == "MAT-002", () => new Material
        {
            Id = Guid.NewGuid(), MaterialCategoryId = category.Id, BaseUnitOfMeasureId = unit.Id,
            Code = "MAT-002", Name = "İnşaat Demiri 12mm", IsActive = true,
        }, ct);

        // 5) Depo
        var warehouse = await GetOrAddAsync(_db.Warehouses, w => w.Code == "WH-001", () => new Warehouse
        {
            Id = Guid.NewGuid(), CompanyId = company.Id, ProjectId = project.Id,
            WarehouseType = WarehouseType.Central, Code = "WH-001", Name = "Merkez Depo", IsActive = true,
        }, ct);

        // 6) Stok bakiyeleri: biri tükenmiş (LowStock = kullanılabilir ≤ 0), biri sağlıklı.
        await EnsureAsync(_db.StockBalances,
            b => b.WarehouseId == warehouse.Id && b.MaterialId == material1.Id,
            () => new StockBalance
            {
                Id = Guid.NewGuid(), WarehouseId = warehouse.Id, MaterialId = material1.Id,
                Quantity = 0m, ReservedQuantity = 0m, TotalCost = 0m, LastRecalculatedAt = DateTime.UtcNow,
            }, ct);
        await EnsureAsync(_db.StockBalances,
            b => b.WarehouseId == warehouse.Id && b.MaterialId == material2.Id,
            () => new StockBalance
            {
                Id = Guid.NewGuid(), WarehouseId = warehouse.Id, MaterialId = material2.Id,
                Quantity = 100m, ReservedQuantity = 0m, TotalCost = 145000m, LastRecalculatedAt = DateTime.UtcNow,
            }, ct);

        // 7) İş emri türü + bir açık + bir kapalı iş emri (WorkOrderProgress = açık sayısı).
        var workOrderType = await GetOrAddAsync(_db.WorkOrderTypes, t => t.Code == "WOT-001", () => new WorkOrderType
        {
            Id = Guid.NewGuid(), Code = "WOT-001", Name = "Saha İşi", IsActive = true,
        }, ct);
        await GetOrAddAsync(_db.WorkOrders, w => w.WorkOrderNo == "WO-001", () => new WorkOrder
        {
            Id = Guid.NewGuid(), WorkOrderTypeId = workOrderType.Id, ProjectId = project.Id,
            Status = WorkOrderStatus.InProgress, WorkOrderNo = "WO-001", Title = "Temel kazısı",
            PlannedStart = DateTime.UtcNow.AddDays(-5), PlannedEnd = DateTime.UtcNow.AddDays(5),
        }, ct);
        await GetOrAddAsync(_db.WorkOrders, w => w.WorkOrderNo == "WO-002", () => new WorkOrder
        {
            Id = Guid.NewGuid(), WorkOrderTypeId = workOrderType.Id, ProjectId = project.Id,
            Status = WorkOrderStatus.Completed, WorkOrderNo = "WO-002", Title = "Saha temizliği",
            PlannedStart = DateTime.UtcNow.AddDays(-20), PlannedEnd = DateTime.UtcNow.AddDays(-10),
        }, ct);

        // 8) Satın alma siparişi + satır (OrderDelivery = onaylı/kısmen teslim).
        var purchaseOrder = await GetOrAddAsync(_db.PurchaseOrders, o => o.OrderNo == "PO-001", () => new PurchaseOrder
        {
            Id = Guid.NewGuid(), SupplierId = supplier.Id, ProjectId = project.Id, CurrencyId = currency.Id,
            Status = PurchaseOrderStatus.Approved, OrderNo = "PO-001", OrderDate = DateTime.UtcNow.AddDays(-3),
        }, ct);
        await EnsureAsync(_db.PurchaseOrderLines,
            l => l.PurchaseOrderId == purchaseOrder.Id,
            () => new PurchaseOrderLine
            {
                Id = Guid.NewGuid(), PurchaseOrderId = purchaseOrder.Id, MaterialId = material1.Id,
                Quantity = 50m, UnitPrice = 120m, CurrencyId = currency.Id, ReceivedQuantity = 0m,
            }, ct);

        // 9) Bütçe + satır + (planı aşan) finansal hareket (BudgetOverrun = aşan bütçe sayısı).
        var budget = await GetOrAddAsync(_db.Budgets, b => b.Name == "PRJ-001 Bütçesi", () => new Budget
        {
            Id = Guid.NewGuid(), ProjectId = project.Id, CurrencyId = currency.Id,
            Name = "PRJ-001 Bütçesi", Year = DateTime.UtcNow.Year, IsActive = true,
        }, ct);
        await EnsureAsync(_db.BudgetLines, l => l.BudgetId == budget.Id, () => new BudgetLine
        {
            Id = Guid.NewGuid(), BudgetId = budget.Id, ProjectId = project.Id,
            Description = "Malzeme bütçesi", PlannedAmount = 100000m,
        }, ct);
        var overrunTx = await GetOrAddAsync(_db.FinancialTransactions, t => t.Description == DemoMarker, () => new FinancialTransaction
        {
            Id = Guid.NewGuid(), TransactionType = FinancialTransactionType.Expense, ProjectId = project.Id,
            CurrencyId = currency.Id, Amount = 130000m, TransactionDate = DateTime.UtcNow.AddDays(-1), Description = DemoMarker,
        }, ct);
        await EnsureAsync(_db.FinancialTransactionLines,
            l => l.FinancialTransactionId == overrunTx.Id,
            () => new FinancialTransactionLine
            {
                Id = Guid.NewGuid(), FinancialTransactionId = overrunTx.Id, ProjectId = project.Id,
                Amount = 130000m, Description = "Gerçekleşen malzeme gideri",
            }, ct);

        // 10) Bekleyen onay talebi (PendingApprovals). Satın alma onay akışının yürürlükteki versiyonunu kullan.
        var pendingExists = await _db.ApprovalRequests.IgnoreQueryFilters()
            .AnyAsync(a => a.RelatedModule == "Procurement" && a.RelatedEntityId == purchaseOrder.Id, ct);
        if (!pendingExists)
        {
            var version = await (from v in _db.ApprovalDefinitionVersions.IgnoreQueryFilters()
                                 join d in _db.ApprovalDefinitions.IgnoreQueryFilters() on v.ApprovalDefinitionId equals d.Id
                                 where d.Code == "PurchaseOrderApproval" && v.IsActive
                                 select v).FirstOrDefaultAsync(ct);
            var requester = await _db.Users.FirstOrDefaultAsync(u => u.UserName == "admin", ct);
            if (version is not null && requester is not null)
            {
                _db.ApprovalRequests.Add(new ApprovalRequest
                {
                    Id = Guid.NewGuid(),
                    ApprovalDefinitionVersionId = version.Id,
                    RelatedModule = "Procurement",
                    RelatedEntityType = "PurchaseOrder",
                    RelatedEntityId = purchaseOrder.Id,
                    RequestedByUserId = requester.Id,
                    Status = ApprovalRequestStatus.Pending,
                    CurrentStepNo = 1,
                });
            }
        }

        await _db.SaveChangesAsync(ct);
    }

    /// <summary>Predicate ile eşleşen kaydı döndürür; yoksa fabrikadan üretip ekler ve kaydederek döndürür.</summary>
    private async Task<TEntity> GetOrAddAsync<TEntity>(
        DbSet<TEntity> set,
        Expression<Func<TEntity, bool>> predicate,
        Func<TEntity> factory,
        CancellationToken ct)
        where TEntity : class
    {
        var existing = await set.IgnoreQueryFilters().FirstOrDefaultAsync(predicate, ct);
        if (existing is not null)
        {
            return existing;
        }

        var entity = factory();
        set.Add(entity);
        await _db.SaveChangesAsync(ct);
        return entity;
    }

    /// <summary>Predicate ile eşleşen kayıt yoksa fabrikadan üretip ekler (kaydı en sonda toplu yapılır).</summary>
    private async Task EnsureAsync<TEntity>(
        DbSet<TEntity> set,
        Expression<Func<TEntity, bool>> predicate,
        Func<TEntity> factory,
        CancellationToken ct)
        where TEntity : class
    {
        if (!await set.IgnoreQueryFilters().AnyAsync(predicate, ct))
        {
            set.Add(factory());
        }
    }
}


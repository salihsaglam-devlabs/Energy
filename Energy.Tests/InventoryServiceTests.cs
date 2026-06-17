using Energy.Shared.Common;
using Energy.Application.Inventory.Services;
using Energy.Domain.BusinessPartners;
using Energy.Domain.Catalog;
using Energy.Domain.Common;
using Energy.Domain.Core;
using Energy.Domain.Inventory;
using Energy.Domain.Procurement;
using Energy.Infrastructure.Inventory.Services;
using Energy.Infrastructure.Persistence;
using Energy.Infrastructure.Procurement.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace Energy.Tests;

/// <summary>
/// Inventory FIFO çekirdeği ve Procurement mal kabul davranış testleri. SQLite
/// in-memory üzerinde gerçek EF Core modeliyle (FK kısıtları açık) çalışır.
/// </summary>
public sealed class InventoryServiceTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly AppDbContext _db;
    private readonly InventoryService _inventory;

    private Guid _uomId, _materialId, _warehouseA, _warehouseB, _supplierId;

    public InventoryServiceTests()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();
        var options = new DbContextOptionsBuilder<AppDbContext>().UseSqlite(_connection).Options;
        _db = new AppDbContext(options);
        _db.Database.EnsureCreated();
        _inventory = new InventoryService(_db, NullLogger<InventoryService>.Instance);
        SeedBaseData();
    }

    [Fact]
    public async Task Fifo_allocates_across_lots_with_correct_total_cost()
    {
        // LOT-001: 500 x 120, LOT-002: 300 x 145
        await _inventory.PostStockInAsync(new StockInRequest(_warehouseA, _materialId, _uomId, 500m, 120m));
        await _inventory.PostStockInAsync(new StockInRequest(_warehouseA, _materialId, _uomId, 300m, 145m));

        // 600 çıkış → 500 (LOT-001) + 100 (LOT-002) = 74.500
        var result = await _inventory.PostStockOutAsync(new StockOutRequest(_warehouseA, _materialId, _uomId, 600m));

        Assert.Equal(74_500m, result.TotalCost);
        Assert.Equal(2, result.Allocations.Count);
        Assert.Equal(500m, result.Allocations[0].Quantity);
        Assert.Equal(120m, result.Allocations[0].UnitCost);
        Assert.Equal(100m, result.Allocations[1].Quantity);
        Assert.Equal(145m, result.Allocations[1].UnitCost);

        var available = await _inventory.GetAvailableQuantityAsync(_warehouseA, _materialId);
        Assert.Equal(200m, available);
    }

    [Fact]
    public async Task Negative_stock_is_blocked()
    {
        await _inventory.PostStockInAsync(new StockInRequest(_warehouseA, _materialId, _uomId, 100m, 50m));

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _inventory.PostStockOutAsync(new StockOutRequest(_warehouseA, _materialId, _uomId, 150m)));
    }

    [Fact]
    public async Task Transfer_moves_quantity_between_warehouses_preserving_cost()
    {
        await _inventory.PostStockInAsync(new StockInRequest(_warehouseA, _materialId, _uomId, 500m, 120m));

        await _inventory.TransferAsync(new StockTransferRequest(_warehouseA, _warehouseB, _materialId, _uomId, 200m));

        Assert.Equal(300m, await _inventory.GetAvailableQuantityAsync(_warehouseA, _materialId));
        Assert.Equal(200m, await _inventory.GetAvailableQuantityAsync(_warehouseB, _materialId));
    }

    [Fact]
    public async Task Rebuild_balances_matches_available_quantity()
    {
        await _inventory.PostStockInAsync(new StockInRequest(_warehouseA, _materialId, _uomId, 500m, 120m));
        await _inventory.PostStockOutAsync(new StockOutRequest(_warehouseA, _materialId, _uomId, 200m));

        await _inventory.RebuildBalancesAsync(_warehouseA, _materialId);

        var balance = await _db.StockBalances
            .FirstAsync(b => b.WarehouseId == _warehouseA && b.MaterialId == _materialId);
        Assert.Equal(300m, balance.Quantity);
    }

    [Fact]
    public async Task GoodsReceipt_creates_stock_in()
    {
        var receipt = new PurchaseReceipt
        {
            Id = Guid.NewGuid(),
            SupplierId = _supplierId,
            WarehouseId = _warehouseA,
            ReceiptNo = "GR-1",
            ReceiptDate = DateTime.UtcNow,
            Status = DocumentStatus.Draft,
        };
        _db.PurchaseReceipts.Add(receipt);
        _db.PurchaseReceiptLines.Add(new PurchaseReceiptLine
        {
            Id = Guid.NewGuid(),
            PurchaseReceiptId = receipt.Id,
            MaterialId = _materialId,
            Quantity = 250m,
            UnitPrice = 130m,
        });
        await _db.SaveChangesAsync();

        var service = new GoodsReceiptService(_db, _inventory, NullLogger<GoodsReceiptService>.Instance);
        await service.ReceiveAsync(receipt.Id);

        Assert.Equal(250m, await _inventory.GetAvailableQuantityAsync(_warehouseA, _materialId));
        Assert.Equal(DocumentStatus.Approved, (await _db.PurchaseReceipts.FindAsync(receipt.Id))!.Status);
    }

    private void SeedBaseData()
    {
        var currency = new Currency { Id = Guid.NewGuid(), Code = "TRY", Name = "TRY", IsActive = true };
        _db.Currencies.Add(currency);

        var company = new Company { Id = Guid.NewGuid(), Code = "C1", Name = "Co", BaseCurrencyId = currency.Id, IsActive = true };
        _db.Companies.Add(company);

        var uom = new UnitOfMeasure { Id = Guid.NewGuid(), Code = "M", Name = "Meter", IsActive = true };
        _db.UnitsOfMeasure.Add(uom);
        _uomId = uom.Id;

        var category = new MaterialCategory { Id = Guid.NewGuid(), Code = "CAT", Name = "Cable", IsActive = true };
        _db.MaterialCategories.Add(category);

        var material = new Material
        {
            Id = Guid.NewGuid(),
            MaterialCategoryId = category.Id,
            BaseUnitOfMeasureId = uom.Id,
            Code = "MAT1",
            Name = "Cable 3x2.5",
            IsActive = true,
        };
        _db.Materials.Add(material);
        _materialId = material.Id;

        var whA = new Warehouse { Id = Guid.NewGuid(), CompanyId = company.Id, WarehouseType = WarehouseType.Central, Code = "WH-A", Name = "Central", IsActive = true };
        var whB = new Warehouse { Id = Guid.NewGuid(), CompanyId = company.Id, WarehouseType = WarehouseType.ProjectSite, Code = "WH-B", Name = "Site", IsActive = true };
        _db.Warehouses.AddRange(whA, whB);
        _warehouseA = whA.Id;
        _warehouseB = whB.Id;

        var supplier = new BusinessPartner { Id = Guid.NewGuid(), PartnerType = PartnerType.Supplier, Code = "SUP1", Name = "Supplier", IsActive = true };
        _db.BusinessPartners.Add(supplier);
        _supplierId = supplier.Id;

        _db.SaveChanges();
    }

    public void Dispose()
    {
        _db.Dispose();
        _connection.Dispose();
    }
}


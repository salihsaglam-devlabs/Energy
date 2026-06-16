using Energy.Shared.Common;
using Energy.Application.Catalog.Services;
using Energy.Application.Inventory.Services;
using Energy.Application.Operations.Services;
using Energy.Domain.Catalog;
using Energy.Domain.Common;
using Energy.Domain.Core;
using Energy.Domain.Inventory;
using Energy.Domain.Operations;
using Energy.Infrastructure.Catalog.Services;
using Energy.Infrastructure.Inventory.Services;
using Energy.Infrastructure.Operations.Services;
using Energy.Infrastructure.Persistence;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace Energy.Tests;

/// <summary>
/// İş kuralı testleri: WorkOrder checklist kapatma engeli/reopen, Inventory reverse,
/// Catalog dinamik öznitelik doğrulaması ve baz birim değiştirme engeli.
/// </summary>
public sealed class BusinessRuleTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly AppDbContext _db;
    private readonly WorkOrderService _workOrders;
    private readonly MaterialService _materials;
    private readonly InventoryService _inventory;

    private Guid _companyId, _uomId;

    public BusinessRuleTests()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();
        var options = new DbContextOptionsBuilder<AppDbContext>().UseSqlite(_connection).Options;
        _db = new AppDbContext(options);
        _db.Database.EnsureCreated();
        _workOrders = new WorkOrderService(_db, NullLogger<WorkOrderService>.Instance);
        _materials = new MaterialService(_db, NullLogger<MaterialService>.Instance);
        _inventory = new InventoryService(_db, NullLogger<InventoryService>.Instance);

        var currency = new Currency { Id = Guid.NewGuid(), Code = "TRY", Name = "TRY", IsActive = true };
        _db.Currencies.Add(currency);
        var company = new Company { Id = Guid.NewGuid(), Code = "C1", Name = "Co", BaseCurrencyId = currency.Id, IsActive = true };
        _db.Companies.Add(company);
        _companyId = company.Id;
        var uom = new UnitOfMeasure { Id = Guid.NewGuid(), Code = "PCS", Name = "Piece", IsActive = true };
        _db.UnitsOfMeasure.Add(uom);
        _uomId = uom.Id;
        _db.SaveChanges();
    }

    [Fact]
    public async Task WorkOrder_cannot_close_with_incomplete_required_checklist()
    {
        var wo = CreateWorkOrder();
        var checklist = new WorkOrderChecklist { Id = Guid.NewGuid(), WorkOrderId = wo.Id, Name = "QC", IsRequired = true };
        _db.WorkOrderChecklists.Add(checklist);
        _db.WorkOrderChecklistItems.Add(new WorkOrderChecklistItem { Id = Guid.NewGuid(), WorkOrderChecklistId = checklist.Id, Description = "Test", IsRequired = true, IsCompleted = false });
        await _db.SaveChangesAsync();

        await Assert.ThrowsAsync<InvalidOperationException>(() => _workOrders.CloseAsync(wo.Id));
    }

    [Fact]
    public async Task WorkOrder_closes_when_required_checklist_completed_and_can_reopen()
    {
        var wo = CreateWorkOrder();
        var checklist = new WorkOrderChecklist { Id = Guid.NewGuid(), WorkOrderId = wo.Id, Name = "QC", IsRequired = true };
        _db.WorkOrderChecklists.Add(checklist);
        _db.WorkOrderChecklistItems.Add(new WorkOrderChecklistItem { Id = Guid.NewGuid(), WorkOrderChecklistId = checklist.Id, Description = "Test", IsRequired = true, IsCompleted = true });
        await _db.SaveChangesAsync();

        await _workOrders.CloseAsync(wo.Id, "done");
        Assert.Equal(WorkOrderStatus.Closed, (await _db.WorkOrders.FindAsync(wo.Id))!.Status);
        Assert.True(await _db.WorkOrderStatusHistories.AnyAsync(h => h.WorkOrderId == wo.Id && h.ToStatus == WorkOrderStatus.Closed));

        await _workOrders.ReopenAsync(wo.Id);
        Assert.Equal(WorkOrderStatus.InProgress, (await _db.WorkOrders.FindAsync(wo.Id))!.Status);
    }

    [Fact]
    public async Task Reverse_stock_in_removes_lot_quantity()
    {
        var (warehouseId, materialId) = SeedWarehouseAndMaterial();
        await _db.SaveChangesAsync();

        var lotId = await _inventory.PostStockInAsync(new StockInRequest(warehouseId, materialId, _uomId, 500m, 120m));
        var lot = await _db.StockLots.FindAsync(lotId);
        var line = await _db.StockDocumentLines.FindAsync(lot!.SourceStockDocumentLineId);

        await _inventory.ReverseDocumentAsync(line!.StockDocumentId);

        Assert.Equal(0m, await _inventory.GetAvailableQuantityAsync(warehouseId, materialId));
        Assert.Equal(DocumentStatus.Cancelled, (await _db.StockDocuments.FindAsync(line.StockDocumentId))!.Status);
    }

    [Fact]
    public async Task Reverse_stock_out_restores_quantity()
    {
        var (warehouseId, materialId) = SeedWarehouseAndMaterial();
        await _db.SaveChangesAsync();

        await _inventory.PostStockInAsync(new StockInRequest(warehouseId, materialId, _uomId, 500m, 120m));
        await _inventory.PostStockOutAsync(new StockOutRequest(warehouseId, materialId, _uomId, 200m));
        Assert.Equal(300m, await _inventory.GetAvailableQuantityAsync(warehouseId, materialId));

        var outTxn = await _db.StockTransactions.FirstAsync(t => t.Quantity < 0);
        await _inventory.ReverseDocumentAsync(outTxn.StockDocumentId);

        Assert.Equal(500m, await _inventory.GetAvailableQuantityAsync(warehouseId, materialId));
    }

    [Fact]
    public async Task Material_activation_requires_mandatory_attribute()
    {
        var category = new MaterialCategory { Id = Guid.NewGuid(), Code = "CAT", Name = "Cat", IsActive = true };
        _db.MaterialCategories.Add(category);
        var def = new MaterialAttributeDefinition { Id = Guid.NewGuid(), Code = "VOLT", Name = "Voltage", DataType = "Text", IsActive = true };
        _db.MaterialAttributeDefinitions.Add(def);
        _db.MaterialCategoryAttributes.Add(new MaterialCategoryAttribute { Id = Guid.NewGuid(), MaterialCategoryId = category.Id, MaterialAttributeDefinitionId = def.Id, IsRequired = true });
        var material = new Material { Id = Guid.NewGuid(), MaterialCategoryId = category.Id, BaseUnitOfMeasureId = _uomId, Code = "M1", Name = "Mat", IsActive = false };
        _db.Materials.Add(material);
        await _db.SaveChangesAsync();

        var errors = await _materials.ValidateAttributesAsync(material.Id);
        Assert.NotEmpty(errors);
        await Assert.ThrowsAsync<InvalidOperationException>(() => _materials.ActivateAsync(material.Id));

        _db.MaterialAttributeValues.Add(new MaterialAttributeValue { Id = Guid.NewGuid(), MaterialId = material.Id, MaterialAttributeDefinitionId = def.Id, ValueText = "400V" });
        await _db.SaveChangesAsync();

        await _materials.ActivateAsync(material.Id);
        Assert.True((await _db.Materials.FindAsync(material.Id))!.IsActive);
    }

    [Fact]
    public async Task Base_unit_cannot_change_after_stock_movement()
    {
        var (warehouseId, materialId) = SeedWarehouseAndMaterial();
        var newUnit = new UnitOfMeasure { Id = Guid.NewGuid(), Code = "BOX", Name = "Box", IsActive = true };
        _db.UnitsOfMeasure.Add(newUnit);
        await _db.SaveChangesAsync();

        // Hareketsiz malzemede değişiklik serbest.
        await _materials.ChangeBaseUnitOfMeasureAsync(materialId, newUnit.Id);

        // Hareket oluştuktan sonra değişiklik engellenir.
        await _inventory.PostStockInAsync(new StockInRequest(warehouseId, materialId, _uomId, 10m, 5m));
        await Assert.ThrowsAsync<InvalidOperationException>(() => _materials.ChangeBaseUnitOfMeasureAsync(materialId, _uomId));
    }

    private WorkOrder CreateWorkOrder()
    {
        var type = new WorkOrderType { Id = Guid.NewGuid(), Code = "WT", Name = "Type", IsActive = true };
        _db.WorkOrderTypes.Add(type);
        var wo = new WorkOrder
        {
            Id = Guid.NewGuid(), WorkOrderTypeId = type.Id, Status = WorkOrderStatus.InProgress,
            WorkOrderNo = "WO-" + Guid.NewGuid().ToString("N")[..6], Title = "Task",
        };
        _db.WorkOrders.Add(wo);
        _db.SaveChanges();
        return wo;
    }

    private (Guid WarehouseId, Guid MaterialId) SeedWarehouseAndMaterial()
    {
        var category = new MaterialCategory { Id = Guid.NewGuid(), Code = "C-" + Guid.NewGuid().ToString("N")[..4], Name = "Cat", IsActive = true };
        _db.MaterialCategories.Add(category);
        var material = new Material { Id = Guid.NewGuid(), MaterialCategoryId = category.Id, BaseUnitOfMeasureId = _uomId, Code = "M-" + Guid.NewGuid().ToString("N")[..4], Name = "Mat", IsActive = true };
        _db.Materials.Add(material);
        var warehouse = new Warehouse { Id = Guid.NewGuid(), CompanyId = _companyId, WarehouseType = WarehouseType.Central, Code = "W-" + Guid.NewGuid().ToString("N")[..4], Name = "WH", IsActive = true };
        _db.Warehouses.Add(warehouse);
        return (warehouse.Id, material.Id);
    }

    public void Dispose()
    {
        _db.Dispose();
        _connection.Dispose();
    }
}


using Energy.Domain.Catalog;
using Energy.Domain.Core;
using Energy.Domain.Inventory;
using Energy.Domain.Projects;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.Enterprise;

/// <summary>Catalog ve Inventory modülleri EF Core yapılandırması.</summary>
public static class CatalogInventoryConfiguration
{
    public static void Configure(ModelBuilder b)
    {
        // ---- Catalog ----
        b.Entity<Brand>(e => { e.ToTable("Brands"); e.HasIndex(x => x.Code).IsUnique(); });

        b.Entity<MaterialCategory>(e =>
        {
            e.ToTable("MaterialCategories");
            e.HasIndex(x => x.Code).IsUnique();
            e.HasOne<MaterialCategory>().WithMany().HasForeignKey(x => x.ParentCategoryId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<MaterialAttributeDefinition>(e => { e.ToTable("MaterialAttributeDefinitions"); e.HasIndex(x => x.Code).IsUnique(); });

        b.Entity<MaterialAttributeOption>(e =>
        {
            e.ToTable("MaterialAttributeOptions");
            e.HasOne<MaterialAttributeDefinition>().WithMany().HasForeignKey(x => x.MaterialAttributeDefinitionId).OnDelete(DeleteBehavior.Cascade);
        });

        b.Entity<MaterialCategoryAttribute>(e =>
        {
            e.ToTable("MaterialCategoryAttributes");
            e.HasIndex(x => new { x.MaterialCategoryId, x.MaterialAttributeDefinitionId }).IsUnique();
            e.HasOne<MaterialCategory>().WithMany().HasForeignKey(x => x.MaterialCategoryId).OnDelete(DeleteBehavior.Cascade);
            e.HasOne<MaterialAttributeDefinition>().WithMany().HasForeignKey(x => x.MaterialAttributeDefinitionId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<Material>(e =>
        {
            e.ToTable("Materials");
            e.HasIndex(x => x.Code).IsUnique();
            e.HasOne<MaterialCategory>().WithMany().HasForeignKey(x => x.MaterialCategoryId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<Brand>().WithMany().HasForeignKey(x => x.BrandId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<UnitOfMeasure>().WithMany().HasForeignKey(x => x.BaseUnitOfMeasureId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<MaterialAttributeValue>(e =>
        {
            e.ToTable("MaterialAttributeValues");
            e.HasIndex(x => new { x.MaterialId, x.MaterialAttributeDefinitionId }).IsUnique();
            e.HasOne<Material>().WithMany().HasForeignKey(x => x.MaterialId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<MaterialAttributeDefinition>().WithMany().HasForeignKey(x => x.MaterialAttributeDefinitionId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<MaterialAttributeOption>().WithMany().HasForeignKey(x => x.OptionId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<MaterialUnitConversion>(e =>
        {
            e.ToTable("MaterialUnitConversions");
            e.HasOne<Material>().WithMany().HasForeignKey(x => x.MaterialId).OnDelete(DeleteBehavior.Cascade);
            e.HasOne<UnitOfMeasure>().WithMany().HasForeignKey(x => x.FromUnitOfMeasureId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<UnitOfMeasure>().WithMany().HasForeignKey(x => x.ToUnitOfMeasureId).OnDelete(DeleteBehavior.Restrict);
        });

        // ---- Inventory ----
        b.Entity<Warehouse>(e =>
        {
            e.ToTable("Warehouses");
            e.HasIndex(x => x.Code).IsUnique();
            e.HasOne<Company>().WithMany().HasForeignKey(x => x.CompanyId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<Branch>().WithMany().HasForeignKey(x => x.BranchId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<Project>().WithMany().HasForeignKey(x => x.ProjectId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<WarehouseLocation>(e =>
        {
            e.ToTable("WarehouseLocations");
            e.HasOne<Warehouse>().WithMany().HasForeignKey(x => x.WarehouseId).OnDelete(DeleteBehavior.Cascade);
            e.HasOne<WarehouseLocation>().WithMany().HasForeignKey(x => x.ParentLocationId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<StockDocumentType>(e => { e.ToTable("StockDocumentTypes"); e.HasIndex(x => x.Code).IsUnique(); });

        b.Entity<StockDocument>(e =>
        {
            e.ToTable("StockDocuments");
            e.HasIndex(x => x.DocumentNo).IsUnique();
            e.HasOne<StockDocumentType>().WithMany().HasForeignKey(x => x.DocumentTypeId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<Warehouse>().WithMany().HasForeignKey(x => x.SourceWarehouseId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<Warehouse>().WithMany().HasForeignKey(x => x.TargetWarehouseId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<Project>().WithMany().HasForeignKey(x => x.ProjectId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<StockDocumentLine>(e =>
        {
            e.ToTable("StockDocumentLines");
            e.HasOne<StockDocument>().WithMany().HasForeignKey(x => x.StockDocumentId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<Material>().WithMany().HasForeignKey(x => x.MaterialId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<UnitOfMeasure>().WithMany().HasForeignKey(x => x.UnitOfMeasureId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<Currency>().WithMany().HasForeignKey(x => x.CurrencyId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<StockLot>(e =>
        {
            e.ToTable("StockLots");
            e.HasOne<Warehouse>().WithMany().HasForeignKey(x => x.WarehouseId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<Material>().WithMany().HasForeignKey(x => x.MaterialId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<StockDocumentLine>().WithMany().HasForeignKey(x => x.SourceStockDocumentLineId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<StockIssueAllocation>(e =>
        {
            e.ToTable("StockIssueAllocations");
            e.HasOne<StockDocumentLine>().WithMany().HasForeignKey(x => x.StockDocumentLineId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<StockLot>().WithMany().HasForeignKey(x => x.StockLotId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<StockTransaction>(e =>
        {
            e.ToTable("StockTransactions");
            e.HasOne<StockDocument>().WithMany().HasForeignKey(x => x.StockDocumentId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<StockDocumentLine>().WithMany().HasForeignKey(x => x.StockDocumentLineId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<StockLot>().WithMany().HasForeignKey(x => x.StockLotId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<Warehouse>().WithMany().HasForeignKey(x => x.WarehouseId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<Material>().WithMany().HasForeignKey(x => x.MaterialId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<StockBalance>(e =>
        {
            e.ToTable("StockBalances");
            e.HasIndex(x => new { x.WarehouseId, x.MaterialId }).IsUnique();
            e.HasOne<Warehouse>().WithMany().HasForeignKey(x => x.WarehouseId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<Material>().WithMany().HasForeignKey(x => x.MaterialId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<StockReservation>(e =>
        {
            e.ToTable("StockReservations");
            e.HasOne<Warehouse>().WithMany().HasForeignKey(x => x.WarehouseId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<Material>().WithMany().HasForeignKey(x => x.MaterialId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<StockCount>(e =>
        {
            e.ToTable("StockCounts");
            e.HasIndex(x => x.CountNo).IsUnique();
            e.HasOne<Warehouse>().WithMany().HasForeignKey(x => x.WarehouseId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<StockCountLine>(e =>
        {
            e.ToTable("StockCountLines");
            e.HasOne<StockCount>().WithMany().HasForeignKey(x => x.StockCountId).OnDelete(DeleteBehavior.Cascade);
            e.HasOne<Material>().WithMany().HasForeignKey(x => x.MaterialId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<WarehouseTransfer>(e =>
        {
            e.ToTable("WarehouseTransfers");
            e.HasIndex(x => x.TransferNo).IsUnique();
            e.HasOne<Warehouse>().WithMany().HasForeignKey(x => x.SourceWarehouseId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<Warehouse>().WithMany().HasForeignKey(x => x.TargetWarehouseId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<WarehouseTransferLine>(e =>
        {
            e.ToTable("WarehouseTransferLines");
            e.HasOne<WarehouseTransfer>().WithMany().HasForeignKey(x => x.WarehouseTransferId).OnDelete(DeleteBehavior.Cascade);
            e.HasOne<Material>().WithMany().HasForeignKey(x => x.MaterialId).OnDelete(DeleteBehavior.Restrict);
        });
    }
}


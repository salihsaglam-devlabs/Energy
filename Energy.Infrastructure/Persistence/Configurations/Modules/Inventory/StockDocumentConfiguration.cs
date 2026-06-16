using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Inventory;

/// <summary>StockDocument EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class StockDocumentConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Inventory.StockDocument>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Inventory.StockDocument> builder)
    {
        builder.ToTable("StockDocuments");
        builder.HasKey(e => e.Id);
        builder.HasOne<global::Energy.Domain.Modules.Inventory.StockDocumentType>().WithMany().HasForeignKey(e => e.DocumentTypeId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<global::Energy.Domain.Modules.Inventory.Warehouse>().WithMany().HasForeignKey(e => e.SourceWarehouseId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<global::Energy.Domain.Modules.Inventory.Warehouse>().WithMany().HasForeignKey(e => e.TargetWarehouseId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<global::Energy.Domain.Modules.Projects.Project>().WithMany().HasForeignKey(e => e.ProjectId).OnDelete(DeleteBehavior.Restrict);
    }
}

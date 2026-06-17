using Energy.Domain.Catalog;
using Energy.Domain.Core;
using Energy.Domain.Inventory;
using Energy.Domain.Projects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.Inventory;

/// <summary>StockDocument EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class StockDocumentConfiguration : IEntityTypeConfiguration<StockDocument>
{
    public void Configure(EntityTypeBuilder<StockDocument> e)
    {
        e.ToTable("StockDocuments");
        e.HasIndex(x => x.DocumentNo).IsUnique();
        e.HasOne<StockDocumentType>().WithMany().HasForeignKey(x => x.DocumentTypeId).OnDelete(DeleteBehavior.Restrict);
        e.HasOne<Warehouse>().WithMany().HasForeignKey(x => x.SourceWarehouseId).OnDelete(DeleteBehavior.Restrict);
        e.HasOne<Warehouse>().WithMany().HasForeignKey(x => x.TargetWarehouseId).OnDelete(DeleteBehavior.Restrict);
        e.HasOne<Project>().WithMany().HasForeignKey(x => x.ProjectId).OnDelete(DeleteBehavior.Restrict);
    }
}

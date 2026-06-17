using Energy.Domain.Catalog;
using Energy.Domain.Core;
using Energy.Domain.Inventory;
using Energy.Domain.Projects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.Catalog;

/// <summary>Material EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class MaterialConfiguration : IEntityTypeConfiguration<Material>
{
    public void Configure(EntityTypeBuilder<Material> e)
    {
        e.ToTable("Materials");
        e.HasIndex(x => x.Code).IsUnique();
        e.HasOne<MaterialCategory>().WithMany().HasForeignKey(x => x.MaterialCategoryId).OnDelete(DeleteBehavior.Restrict);
        e.HasOne<Brand>().WithMany().HasForeignKey(x => x.BrandId).OnDelete(DeleteBehavior.Restrict);
        e.HasOne<UnitOfMeasure>().WithMany().HasForeignKey(x => x.BaseUnitOfMeasureId).OnDelete(DeleteBehavior.Restrict);
    }
}

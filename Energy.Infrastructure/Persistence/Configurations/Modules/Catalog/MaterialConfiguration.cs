using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Catalog;

/// <summary>Material EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class MaterialConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Catalog.Material>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Catalog.Material> builder)
    {
        builder.ToTable("Materials");
        builder.HasKey(e => e.Id);
        builder.HasOne<global::Energy.Domain.Modules.Catalog.MaterialCategory>().WithMany().HasForeignKey(e => e.MaterialCategoryId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<global::Energy.Domain.Modules.Catalog.Brand>().WithMany().HasForeignKey(e => e.BrandId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<global::Energy.Domain.Modules.Core.UnitOfMeasure>().WithMany().HasForeignKey(e => e.BaseUnitOfMeasureId).OnDelete(DeleteBehavior.Restrict);
    }
}

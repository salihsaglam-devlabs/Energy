using Energy.Domain.Catalog;
using Energy.Domain.Core;
using Energy.Domain.Inventory;
using Energy.Domain.Projects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.Catalog;

/// <summary>MaterialUnitConversion EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class MaterialUnitConversionConfiguration : IEntityTypeConfiguration<MaterialUnitConversion>
{
    public void Configure(EntityTypeBuilder<MaterialUnitConversion> e)
    {
        e.ToTable("MaterialUnitConversions");
        e.HasOne<Material>().WithMany().HasForeignKey(x => x.MaterialId).OnDelete(DeleteBehavior.Cascade);
        e.HasOne<UnitOfMeasure>().WithMany().HasForeignKey(x => x.FromUnitOfMeasureId).OnDelete(DeleteBehavior.Restrict);
        e.HasOne<UnitOfMeasure>().WithMany().HasForeignKey(x => x.ToUnitOfMeasureId).OnDelete(DeleteBehavior.Restrict);
    }
}

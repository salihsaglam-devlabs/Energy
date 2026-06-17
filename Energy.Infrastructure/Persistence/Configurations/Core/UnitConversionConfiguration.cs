using Energy.Domain.Core;
using Energy.Domain.IAM;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.Core;

/// <summary>UnitConversion EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class UnitConversionConfiguration : IEntityTypeConfiguration<UnitConversion>
{
    public void Configure(EntityTypeBuilder<UnitConversion> e)
    {
        e.ToTable("UnitConversions");
        e.HasOne<UnitOfMeasure>().WithMany().HasForeignKey(x => x.FromUnitOfMeasureId).OnDelete(DeleteBehavior.Restrict);
        e.HasOne<UnitOfMeasure>().WithMany().HasForeignKey(x => x.ToUnitOfMeasureId).OnDelete(DeleteBehavior.Restrict);
    }
}

using Energy.Domain.Core;
using Energy.Domain.IAM;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.Core;

/// <summary>UnitOfMeasure EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class UnitOfMeasureConfiguration : IEntityTypeConfiguration<UnitOfMeasure>
{
    public void Configure(EntityTypeBuilder<UnitOfMeasure> e)
    {
        e.ToTable("UnitsOfMeasure");
        e.HasIndex(x => x.Code).IsUnique();
    }
}

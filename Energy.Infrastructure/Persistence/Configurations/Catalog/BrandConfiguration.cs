using Energy.Domain.Catalog;
using Energy.Domain.Core;
using Energy.Domain.Inventory;
using Energy.Domain.Projects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.Catalog;

/// <summary>Brand EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class BrandConfiguration : IEntityTypeConfiguration<Brand>
{
    public void Configure(EntityTypeBuilder<Brand> e)
    {
        e.ToTable("Brands"); e.HasIndex(x => x.Code).IsUnique(); 
    }
}

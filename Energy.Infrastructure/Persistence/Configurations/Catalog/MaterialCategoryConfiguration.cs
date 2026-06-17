using Energy.Domain.Catalog;
using Energy.Domain.Core;
using Energy.Domain.Inventory;
using Energy.Domain.Projects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.Catalog;

/// <summary>MaterialCategory EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class MaterialCategoryConfiguration : IEntityTypeConfiguration<MaterialCategory>
{
    public void Configure(EntityTypeBuilder<MaterialCategory> e)
    {
        e.ToTable("MaterialCategories");
        e.HasIndex(x => x.Code).IsUnique();
        e.HasOne<MaterialCategory>().WithMany().HasForeignKey(x => x.ParentCategoryId).OnDelete(DeleteBehavior.Restrict);
    }
}

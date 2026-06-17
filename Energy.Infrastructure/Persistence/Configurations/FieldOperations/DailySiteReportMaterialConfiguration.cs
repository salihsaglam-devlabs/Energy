using Energy.Domain.Assets;
using Energy.Domain.Catalog;
using Energy.Domain.Contracts;
using Energy.Domain.Core;
using Energy.Domain.FieldOperations;
using Energy.Domain.HR;
using Energy.Domain.IAM;
using Energy.Domain.Inventory;
using Energy.Domain.Operations;
using Energy.Domain.Organization;
using Energy.Domain.Projects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.FieldOperations;

/// <summary>DailySiteReportMaterial EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class DailySiteReportMaterialConfiguration : IEntityTypeConfiguration<DailySiteReportMaterial>
{
    public void Configure(EntityTypeBuilder<DailySiteReportMaterial> e)
    {
        e.ToTable("DailySiteReportMaterials");
        e.HasOne<DailySiteReport>().WithMany().HasForeignKey(x => x.DailySiteReportId).OnDelete(DeleteBehavior.Cascade);
        e.HasOne<Material>().WithMany().HasForeignKey(x => x.MaterialId).OnDelete(DeleteBehavior.Restrict);
    }
}

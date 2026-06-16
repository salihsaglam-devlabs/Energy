using Energy.Domain.Modules.Assets;
using Energy.Domain.Modules.Catalog;
using Energy.Domain.Modules.Contracts;
using Energy.Domain.Modules.Core;
using Energy.Domain.Modules.FieldOperations;
using Energy.Domain.Modules.HR;
using Energy.Domain.Modules.IAM;
using Energy.Domain.Modules.Inventory;
using Energy.Domain.Modules.Operations;
using Energy.Domain.Modules.Organization;
using Energy.Domain.Modules.Projects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.FieldOperations;

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

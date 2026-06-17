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

namespace Energy.Infrastructure.Persistence.Configurations.Operations;

/// <summary>WorkOrderMaterialUsage EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class WorkOrderMaterialUsageConfiguration : IEntityTypeConfiguration<WorkOrderMaterialUsage>
{
    public void Configure(EntityTypeBuilder<WorkOrderMaterialUsage> e)
    {
        e.ToTable("WorkOrderMaterialUsages");
        e.HasOne<WorkOrder>().WithMany().HasForeignKey(x => x.WorkOrderId).OnDelete(DeleteBehavior.Restrict);
        e.HasOne<StockDocumentLine>().WithMany().HasForeignKey(x => x.StockDocumentLineId).OnDelete(DeleteBehavior.Restrict);
        e.HasOne<Material>().WithMany().HasForeignKey(x => x.MaterialId).OnDelete(DeleteBehavior.Restrict);
    }
}

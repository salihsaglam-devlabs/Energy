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

namespace Energy.Infrastructure.Persistence.Configurations.Assets;

/// <summary>EquipmentMaintenance EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class EquipmentMaintenanceConfiguration : IEntityTypeConfiguration<EquipmentMaintenance>
{
    public void Configure(EntityTypeBuilder<EquipmentMaintenance> e)
    {
        e.ToTable("EquipmentMaintenances");
        e.HasOne<EquipmentAsset>().WithMany().HasForeignKey(x => x.EquipmentAssetId).OnDelete(DeleteBehavior.Cascade);
    }
}

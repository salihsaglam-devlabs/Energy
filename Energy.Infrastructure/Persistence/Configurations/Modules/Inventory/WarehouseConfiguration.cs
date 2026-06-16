using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Inventory;

/// <summary>Warehouse EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class WarehouseConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Inventory.Warehouse>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Inventory.Warehouse> builder)
    {
        builder.ToTable("Warehouses");
        builder.HasKey(e => e.Id);
        builder.HasOne<global::Energy.Domain.Modules.Core.Company>().WithMany().HasForeignKey(e => e.CompanyId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<global::Energy.Domain.Modules.Core.Branch>().WithMany().HasForeignKey(e => e.BranchId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<global::Energy.Domain.Modules.Projects.Project>().WithMany().HasForeignKey(e => e.ProjectId).OnDelete(DeleteBehavior.Restrict);
    }
}

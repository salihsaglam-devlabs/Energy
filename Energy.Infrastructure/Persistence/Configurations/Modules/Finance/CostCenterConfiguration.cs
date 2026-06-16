using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Finance;

/// <summary>CostCenter EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class CostCenterConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Finance.CostCenter>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Finance.CostCenter> builder)
    {
        builder.ToTable("CostCenters");
        builder.HasKey(e => e.Id);
    }
}

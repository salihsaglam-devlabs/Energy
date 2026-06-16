using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Finance;

/// <summary>CollectionAllocation EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class CollectionAllocationConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Finance.CollectionAllocation>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Finance.CollectionAllocation> builder)
    {
        builder.ToTable("CollectionAllocations");
        builder.HasKey(e => e.Id);
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Finance;

/// <summary>Collection EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class CollectionConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Finance.Collection>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Finance.Collection> builder)
    {
        builder.ToTable("Collections");
        builder.HasKey(e => e.Id);
    }
}

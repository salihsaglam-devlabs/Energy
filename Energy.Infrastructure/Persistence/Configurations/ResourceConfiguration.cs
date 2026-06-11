using Energy.Domain.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations;

public sealed class ResourceConfiguration : IEntityTypeConfiguration<Resource>
{
    public void Configure(EntityTypeBuilder<Resource> builder)
    {
        builder.ToTable("Resources");
        builder.HasQueryFilter(entry => !entry.IsDeleted);

        builder.Property(entry => entry.Key).HasMaxLength(200).IsRequired();
        builder.Property(entry => entry.Culture).HasMaxLength(15).IsRequired();
        builder.Property(entry => entry.Value).IsRequired();

        // A localization key can only have a single value per culture.
        builder.HasIndex(entry => new { entry.Key, entry.Culture })
            .IsUnique()
            .HasDatabaseName("IX_LocalizationEntries_Key_Culture");
    }
}


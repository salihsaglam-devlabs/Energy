using Energy.Domain.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations;

public sealed class ResourceConfiguration : IEntityTypeConfiguration<Resource>
{
    public void Configure(EntityTypeBuilder<Resource> builder)
    {
        builder.ToTable("LocalizationResources");
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Key).IsRequired().HasMaxLength(200);
        builder.Property(r => r.Culture).IsRequired().HasMaxLength(10);
        builder.Property(r => r.Value).IsRequired();

        builder.HasIndex(r => new { r.Culture, r.Key }).IsUnique();
        builder.HasIndex(r => r.Key);
    }
}

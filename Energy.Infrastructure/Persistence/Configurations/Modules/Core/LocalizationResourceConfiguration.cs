using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Core;

/// <summary>LocalizationResource EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class LocalizationResourceConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Core.LocalizationResource>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Core.LocalizationResource> builder)
    {
        builder.ToTable("LocalizationResources");
        builder.HasKey(e => e.Id);
    }
}

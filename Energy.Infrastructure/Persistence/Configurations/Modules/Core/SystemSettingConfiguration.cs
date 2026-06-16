using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Core;

/// <summary>SystemSetting EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class SystemSettingConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Core.SystemSetting>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Core.SystemSetting> builder)
    {
        builder.ToTable("SystemSettings");
        builder.HasKey(e => e.Id);
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.IAM;

/// <summary>UserSetting EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class UserSettingConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.IAM.UserSetting>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.IAM.UserSetting> builder)
    {
        builder.ToTable("UserSettings");
        builder.HasKey(e => e.Id);
    }
}

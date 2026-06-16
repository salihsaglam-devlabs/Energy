using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Notifications;

/// <summary>NotificationPreference EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class NotificationPreferenceConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Notifications.NotificationPreference>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Notifications.NotificationPreference> builder)
    {
        builder.ToTable("NotificationPreferences");
        builder.HasKey(e => e.Id);
    }
}

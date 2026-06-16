using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Notifications;

/// <summary>Notification EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class NotificationConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Notifications.Notification>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Notifications.Notification> builder)
    {
        builder.ToTable("Notifications");
        builder.HasKey(e => e.Id);
    }
}

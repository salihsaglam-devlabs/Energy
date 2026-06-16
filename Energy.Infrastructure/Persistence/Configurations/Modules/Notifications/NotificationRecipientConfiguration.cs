using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Notifications;

/// <summary>NotificationRecipient EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class NotificationRecipientConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Notifications.NotificationRecipient>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Notifications.NotificationRecipient> builder)
    {
        builder.ToTable("NotificationRecipients");
        builder.HasKey(e => e.Id);
        builder.HasOne<global::Energy.Domain.Modules.Notifications.Notification>().WithMany().HasForeignKey(e => e.NotificationId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne<global::Energy.Domain.Modules.IAM.User>().WithMany().HasForeignKey(e => e.UserId).OnDelete(DeleteBehavior.Restrict);
    }
}

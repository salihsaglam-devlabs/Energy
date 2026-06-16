using Energy.Domain.Modules.Documents;
using Energy.Domain.Modules.IAM;
using Energy.Domain.Modules.Notifications;
using Energy.Domain.Modules.Reporting;
using Energy.Domain.Modules.Workflow;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Notifications;

/// <summary>NotificationRecipient EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class NotificationRecipientConfiguration : IEntityTypeConfiguration<NotificationRecipient>
{
    public void Configure(EntityTypeBuilder<NotificationRecipient> e)
    {
        e.ToTable("NotificationRecipients");
        e.HasIndex(x => new { x.UserId, x.IsRead });
        e.HasOne<Notification>().WithMany().HasForeignKey(x => x.NotificationId).OnDelete(DeleteBehavior.Cascade);
        e.HasOne<User>().WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
    }
}

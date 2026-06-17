using Energy.Domain.Documents;
using Energy.Domain.IAM;
using Energy.Domain.Notifications;
using Energy.Domain.Reporting;
using Energy.Domain.Workflow;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.Notifications;

/// <summary>NotificationPreference EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class NotificationPreferenceConfiguration : IEntityTypeConfiguration<NotificationPreference>
{
    public void Configure(EntityTypeBuilder<NotificationPreference> e)
    {
        e.ToTable("NotificationPreferences");
        e.HasIndex(x => new { x.UserId, x.NotificationType }).IsUnique();
        e.HasOne<User>().WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
    }
}

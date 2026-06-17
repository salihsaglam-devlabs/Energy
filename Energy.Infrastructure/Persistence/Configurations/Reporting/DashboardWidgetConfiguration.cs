using Energy.Domain.Documents;
using Energy.Domain.IAM;
using Energy.Domain.Notifications;
using Energy.Domain.Reporting;
using Energy.Domain.Workflow;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.Reporting;

/// <summary>DashboardWidget EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class DashboardWidgetConfiguration : IEntityTypeConfiguration<DashboardWidget>
{
    public void Configure(EntityTypeBuilder<DashboardWidget> e)
    {
        e.ToTable("DashboardWidgets");
        e.HasIndex(x => x.Code).IsUnique();
        e.HasOne<Permission>().WithMany().HasForeignKey(x => x.RequiredPermissionCode)
            .HasPrincipalKey(p => p.Code).OnDelete(DeleteBehavior.SetNull);
    }
}

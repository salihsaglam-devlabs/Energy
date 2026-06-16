using Energy.Domain.Modules.Documents;
using Energy.Domain.Modules.IAM;
using Energy.Domain.Modules.Notifications;
using Energy.Domain.Modules.Reporting;
using Energy.Domain.Modules.Workflow;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Reporting;

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

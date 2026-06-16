using Energy.Domain.Documents;
using Energy.Domain.IAM;
using Energy.Domain.Notifications;
using Energy.Domain.Reporting;
using Energy.Domain.Workflow;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.Reporting;

/// <summary>ReportDefinition EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class ReportDefinitionConfiguration : IEntityTypeConfiguration<ReportDefinition>
{
    public void Configure(EntityTypeBuilder<ReportDefinition> e)
    {
        e.ToTable("ReportDefinitions");
        e.HasIndex(x => x.Code).IsUnique();
        e.HasOne<Permission>().WithMany().HasForeignKey(x => x.RequiredPermissionCode)
            .HasPrincipalKey(p => p.Code).OnDelete(DeleteBehavior.SetNull);
    }
}

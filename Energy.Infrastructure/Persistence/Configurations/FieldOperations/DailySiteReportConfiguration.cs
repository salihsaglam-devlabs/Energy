using Energy.Domain.Assets;
using Energy.Domain.Catalog;
using Energy.Domain.Contracts;
using Energy.Domain.Core;
using Energy.Domain.FieldOperations;
using Energy.Domain.HR;
using Energy.Domain.IAM;
using Energy.Domain.Inventory;
using Energy.Domain.Operations;
using Energy.Domain.Organization;
using Energy.Domain.Projects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.FieldOperations;

/// <summary>DailySiteReport EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class DailySiteReportConfiguration : IEntityTypeConfiguration<DailySiteReport>
{
    public void Configure(EntityTypeBuilder<DailySiteReport> e)
    {
        e.ToTable("DailySiteReports");
        e.HasIndex(x => x.ReportNo).IsUnique();
        e.HasOne<Project>().WithMany().HasForeignKey(x => x.ProjectId).OnDelete(DeleteBehavior.Restrict);
        e.HasOne<WorkOrder>().WithMany().HasForeignKey(x => x.WorkOrderId).OnDelete(DeleteBehavior.Restrict);
    }
}

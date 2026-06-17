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

namespace Energy.Infrastructure.Persistence.Configurations.HR;

/// <summary>TimesheetLine EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class TimesheetLineConfiguration : IEntityTypeConfiguration<TimesheetLine>
{
    public void Configure(EntityTypeBuilder<TimesheetLine> e)
    {
        e.ToTable("TimesheetLines");
        e.HasOne<Timesheet>().WithMany().HasForeignKey(x => x.TimesheetId).OnDelete(DeleteBehavior.Cascade);
        e.HasOne<Employee>().WithMany().HasForeignKey(x => x.EmployeeId).OnDelete(DeleteBehavior.Restrict);
        e.HasOne<Project>().WithMany().HasForeignKey(x => x.ProjectId).OnDelete(DeleteBehavior.Restrict);
        e.HasOne<WorkOrder>().WithMany().HasForeignKey(x => x.WorkOrderId).OnDelete(DeleteBehavior.Restrict);
    }
}

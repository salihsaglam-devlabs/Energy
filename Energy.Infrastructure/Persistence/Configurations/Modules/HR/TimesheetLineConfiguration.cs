using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.HR;

/// <summary>TimesheetLine EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class TimesheetLineConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.HR.TimesheetLine>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.HR.TimesheetLine> builder)
    {
        builder.ToTable("TimesheetLines");
        builder.HasKey(e => e.Id);
        builder.HasOne<global::Energy.Domain.Modules.HR.Timesheet>().WithMany().HasForeignKey(e => e.TimesheetId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne<global::Energy.Domain.Modules.Organization.Employee>().WithMany().HasForeignKey(e => e.EmployeeId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<global::Energy.Domain.Modules.Projects.Project>().WithMany().HasForeignKey(e => e.ProjectId).OnDelete(DeleteBehavior.Restrict);
    }
}

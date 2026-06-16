using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.HR;

/// <summary>Timesheet EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class TimesheetConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.HR.Timesheet>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.HR.Timesheet> builder)
    {
        builder.ToTable("Timesheets");
        builder.HasKey(e => e.Id);
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.FieldOperations;

/// <summary>DailySiteReportWorker EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class DailySiteReportWorkerConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.FieldOperations.DailySiteReportWorker>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.FieldOperations.DailySiteReportWorker> builder)
    {
        builder.ToTable("DailySiteReportWorkers");
        builder.HasKey(e => e.Id);
        builder.HasOne<global::Energy.Domain.Modules.FieldOperations.DailySiteReport>().WithMany().HasForeignKey(e => e.DailySiteReportId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne<global::Energy.Domain.Modules.Organization.Employee>().WithMany().HasForeignKey(e => e.EmployeeId).OnDelete(DeleteBehavior.Restrict);
    }
}

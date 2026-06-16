using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.FieldOperations;

/// <summary>DailySiteReport EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class DailySiteReportConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.FieldOperations.DailySiteReport>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.FieldOperations.DailySiteReport> builder)
    {
        builder.ToTable("DailySiteReports");
        builder.HasKey(e => e.Id);
        builder.HasOne<global::Energy.Domain.Modules.Projects.Project>().WithMany().HasForeignKey(e => e.ProjectId).OnDelete(DeleteBehavior.Restrict);
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Reporting;

/// <summary>ReportDefinition EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class ReportDefinitionConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Reporting.ReportDefinition>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Reporting.ReportDefinition> builder)
    {
        builder.ToTable("ReportDefinitions");
        builder.HasKey(e => e.Id);
    }
}

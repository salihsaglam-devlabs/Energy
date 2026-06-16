using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Reporting;

/// <summary>DashboardWidget EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class DashboardWidgetConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Reporting.DashboardWidget>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Reporting.DashboardWidget> builder)
    {
        builder.ToTable("DashboardWidgets");
        builder.HasKey(e => e.Id);
    }
}

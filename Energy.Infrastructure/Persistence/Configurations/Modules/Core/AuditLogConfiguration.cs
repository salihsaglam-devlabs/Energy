using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Core;

/// <summary>AuditLog EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class AuditLogConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Core.AuditLog>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Core.AuditLog> builder)
    {
        builder.ToTable("AuditLogs");
        builder.HasKey(e => e.Id);
    }
}

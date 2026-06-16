using Energy.Domain.Modules.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations;

public sealed class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.ToTable("AuditLogs");
        builder.HasKey(l => l.Id);

        builder.Property(l => l.Id).ValueGeneratedOnAdd();
        builder.Property(l => l.UserName).HasMaxLength(150);
        builder.Property(l => l.IpAddress).HasMaxLength(45);
        builder.Property(l => l.HttpMethod).HasMaxLength(10);
        builder.Property(l => l.Path).HasMaxLength(500);
        builder.Property(l => l.QueryString).HasMaxLength(2000);
        builder.Property(l => l.Source).HasMaxLength(10);
        // Açık sütun türü yok: sınırsız uzunluktaki dizeler, sağlayıcının büyük metin
        // türüne otomatik eşlenir (PostgreSQL "text", SQL Server "nvarchar(max)").
        builder.Property(l => l.ExceptionType).HasMaxLength(200);

        builder.HasIndex(l => l.OccurredAt).IsDescending();
        builder.HasIndex(l => l.UserId);
        builder.HasIndex(l => l.CorrelationId);
        builder.HasIndex(l => l.Source);
        builder.HasIndex(l => new { l.HttpMethod, l.Path });
    }
}

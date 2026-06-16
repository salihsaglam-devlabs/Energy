using Energy.Domain.Modules.IAM;
using Energy.Domain.Modules.IAM;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations;

public sealed class ApiEndpointConfiguration : IEntityTypeConfiguration<ApiEndpoint>
{
    public void Configure(EntityTypeBuilder<ApiEndpoint> builder)
    {
        builder.ToTable("ApiEndpoints");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name).IsRequired().HasMaxLength(150);
        builder.Property(e => e.Description).HasMaxLength(500);
        builder.Property(e => e.Path).IsRequired().HasMaxLength(300);
        builder.Property(e => e.HttpMethod).IsRequired().HasMaxLength(10);
        builder.Property(e => e.RequiredPermissionCode).HasMaxLength(150);

        builder.HasOne<Permission>().WithMany()
            .HasForeignKey(e => e.RequiredPermissionCode)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(e => new { e.HttpMethod, e.Path }).IsUnique();
        builder.HasIndex(e => e.RequiredPermissionCode);

        builder.HasQueryFilter(e => !e.IsDeleted);
    }
}

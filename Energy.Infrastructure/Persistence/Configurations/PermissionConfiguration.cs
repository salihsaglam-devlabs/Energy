using Energy.Domain.IAM;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations;

public sealed class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("Permissions");
        builder.HasKey(p => p.Code);

        builder.Property(p => p.Code).HasMaxLength(150);
        builder.Property(p => p.Module).IsRequired().HasMaxLength(50);
        builder.Property(p => p.Action).IsRequired().HasMaxLength(50);
        builder.Property(p => p.DisplayNameKey).IsRequired().HasMaxLength(200);
        builder.Property(p => p.DescriptionKey).HasMaxLength(200);

        builder.HasIndex(p => p.Module);
    }
}

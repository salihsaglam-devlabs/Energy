using Energy.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations;

public sealed class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("Permissions");
        builder.HasQueryFilter(permission => !permission.IsDeleted);
        builder.HasIndex(permission => permission.Code).IsUnique();
        builder.Property(permission => permission.Code).HasMaxLength(100).IsRequired();
        builder.Property(permission => permission.Name).HasMaxLength(200).IsRequired();
    }
}

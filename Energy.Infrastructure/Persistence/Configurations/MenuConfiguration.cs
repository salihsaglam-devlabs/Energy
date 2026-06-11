using Energy.Domain.Identity;
using Energy.Domain.System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations;

public sealed class MenuConfiguration : IEntityTypeConfiguration<Menu>
{
    public void Configure(EntityTypeBuilder<Menu> builder)
    {
        builder.ToTable("Menus");
        builder.HasKey(m => m.Id);

        builder.Property(m => m.NameKey).IsRequired().HasMaxLength(200);
        builder.Property(m => m.Url).HasMaxLength(300);
        builder.Property(m => m.Icon).HasMaxLength(100);
        builder.Property(m => m.RequiredPermissionCode).HasMaxLength(150);

        builder.HasOne<Menu>().WithMany().HasForeignKey(m => m.ParentId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<Permission>().WithMany()
            .HasForeignKey(m => m.RequiredPermissionCode)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(m => m.ParentId);
        builder.HasIndex(m => m.RequiredPermissionCode);
        builder.HasIndex(m => new { m.ParentId, m.DisplayOrder });

        builder.HasQueryFilter(m => !m.IsDeleted);
    }
}

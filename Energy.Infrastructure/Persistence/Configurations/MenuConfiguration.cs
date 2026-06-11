using Energy.Domain.System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations;

public sealed class MenuConfiguration : IEntityTypeConfiguration<Menu>
{
    public void Configure(EntityTypeBuilder<Menu> builder)
    {
        builder.ToTable("Menus");
        builder.HasQueryFilter(menu => !menu.IsDeleted);
        builder.Property(menu => menu.Name).HasMaxLength(150).IsRequired();
        builder.Property(menu => menu.Url).HasMaxLength(300).IsRequired();
        builder.Property(menu => menu.Icon).HasMaxLength(100).IsRequired();
    }
}

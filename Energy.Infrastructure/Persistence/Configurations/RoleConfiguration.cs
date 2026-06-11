using Energy.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations;

public sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles");
        builder.HasKey(role => role.Id).HasName("PK_Roles");
        builder.Property(role => role.Id).ValueGeneratedNever();
        builder.Property(role => role.Description).HasColumnType("text").IsRequired();
        builder.Property(role => role.Name).HasMaxLength(256);
        builder.Property(role => role.NormalizedName).HasMaxLength(256);
        builder.Property(role => role.ConcurrencyStamp).HasColumnType("text");
        builder.HasIndex(role => role.NormalizedName)
            .IsUnique()
            .HasDatabaseName("RoleNameIndex");
    }
}

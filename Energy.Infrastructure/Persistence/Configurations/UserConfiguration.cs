using Energy.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(user => user.Id).HasName("PK_Users");
        builder.Property(user => user.Id).ValueGeneratedNever();
        builder.Property(user => user.FirstName).HasColumnType("text").IsRequired();
        builder.Property(user => user.LastName).HasColumnType("text").IsRequired();
        builder.Property(user => user.UserName).HasMaxLength(256);
        builder.Property(user => user.NormalizedUserName).HasMaxLength(256);
        builder.Property(user => user.Email).HasMaxLength(256);
        builder.Property(user => user.NormalizedEmail).HasMaxLength(256);
        builder.Property(user => user.PasswordHash).HasColumnType("text");
        builder.Property(user => user.SecurityStamp).HasColumnType("text");
        builder.Property(user => user.ConcurrencyStamp).HasColumnType("text");
        builder.Property(user => user.PhoneNumber).HasColumnType("text");
        builder.Property(user => user.LockoutEnd).HasColumnType("timestamp with time zone");
        builder.Property(user => user.ProfileImage).HasColumnType("bytea");
        builder.Property(user => user.ProfileImageContentType).HasMaxLength(100);
        builder.HasIndex(user => user.NormalizedEmail)
            .HasDatabaseName("EmailIndex");
        builder.HasIndex(user => user.NormalizedUserName)
            .IsUnique()
            .HasDatabaseName("UserNameIndex");
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.IAM;

/// <summary>UserRole EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class UserRoleConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.IAM.UserRole>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.IAM.UserRole> builder)
    {
        builder.ToTable("UserRoles");
        builder.HasKey(e => e.Id);
        builder.HasOne<global::Energy.Domain.Modules.IAM.User>().WithMany().HasForeignKey(e => e.UserId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne<global::Energy.Domain.Modules.IAM.Role>().WithMany().HasForeignKey(e => e.RoleId).OnDelete(DeleteBehavior.Restrict);
    }
}

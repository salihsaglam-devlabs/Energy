using Energy.Domain.Identity;
using Energy.Domain.System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations;

public sealed class AccessRulePermissionConfiguration : IEntityTypeConfiguration<AccessRulePermission>
{
    public void Configure(EntityTypeBuilder<AccessRulePermission> builder)
    {
        builder.ToTable("AccessRulePermissions");
        builder.HasKey(x => new { x.AccessRuleId, x.PermissionId }).HasName("PK_AccessRulePermissions");
        builder.HasIndex(x => x.PermissionId).HasDatabaseName("IX_AccessRulePermissions_PermissionId");
        builder.HasOne<AccessRule>()
            .WithMany()
            .HasForeignKey(x => x.AccessRuleId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_AccessRulePermissions_AccessRules_AccessRuleId");
        builder.HasOne<Permission>()
            .WithMany()
            .HasForeignKey(x => x.PermissionId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_AccessRulePermissions_Permissions_PermissionId");
    }
}


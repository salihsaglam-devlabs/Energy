using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.IAM;

/// <summary>Permission EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class PermissionConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.IAM.Permission>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.IAM.Permission> builder)
    {
        builder.ToTable("Permissions");
        builder.HasKey(e => e.Id);
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.IAM;

/// <summary>Role EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class RoleConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.IAM.Role>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.IAM.Role> builder)
    {
        builder.ToTable("Roles");
        builder.HasKey(e => e.Id);
    }
}

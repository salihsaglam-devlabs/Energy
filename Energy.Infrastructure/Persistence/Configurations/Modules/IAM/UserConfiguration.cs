using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.IAM;

/// <summary>User EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class UserConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.IAM.User>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.IAM.User> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(e => e.Id);
    }
}

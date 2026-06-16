using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.IAM;

/// <summary>Menu EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class MenuConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.IAM.Menu>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.IAM.Menu> builder)
    {
        builder.ToTable("Menus");
        builder.HasKey(e => e.Id);
        builder.HasOne<global::Energy.Domain.Modules.IAM.Menu>().WithMany().HasForeignKey(e => e.ParentId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<global::Energy.Domain.Modules.IAM.Permission>().WithMany().HasForeignKey(e => e.RequiredPermissionCode).HasPrincipalKey("Code").OnDelete(DeleteBehavior.SetNull);
        builder.HasOne<global::Energy.Domain.Modules.Core.LocalizationResource>().WithMany().HasForeignKey(e => e.NameKey).HasPrincipalKey("Key").OnDelete(DeleteBehavior.Restrict);
    }
}

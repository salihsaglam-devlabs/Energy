using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Documents;

/// <summary>DocumentPermission EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class DocumentPermissionConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Documents.DocumentPermission>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Documents.DocumentPermission> builder)
    {
        builder.ToTable("DocumentPermissions");
        builder.HasKey(e => e.Id);
    }
}

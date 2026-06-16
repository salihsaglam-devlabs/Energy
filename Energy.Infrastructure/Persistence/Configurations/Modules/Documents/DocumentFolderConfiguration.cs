using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Documents;

/// <summary>DocumentFolder EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class DocumentFolderConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Documents.DocumentFolder>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Documents.DocumentFolder> builder)
    {
        builder.ToTable("DocumentFolders");
        builder.HasKey(e => e.Id);
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Documents;

/// <summary>DocumentVersion EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class DocumentVersionConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Documents.DocumentVersion>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Documents.DocumentVersion> builder)
    {
        builder.ToTable("DocumentVersions");
        builder.HasKey(e => e.Id);
        builder.HasOne<global::Energy.Domain.Modules.Documents.Document>().WithMany().HasForeignKey(e => e.DocumentId).OnDelete(DeleteBehavior.Cascade);
    }
}

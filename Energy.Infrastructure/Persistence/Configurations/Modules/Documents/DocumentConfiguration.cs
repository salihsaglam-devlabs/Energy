using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Documents;

/// <summary>Document EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class DocumentConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Documents.Document>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Documents.Document> builder)
    {
        builder.ToTable("Documents");
        builder.HasKey(e => e.Id);
    }
}

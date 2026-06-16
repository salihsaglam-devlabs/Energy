using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Documents;

/// <summary>DocumentRelation EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class DocumentRelationConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Documents.DocumentRelation>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Documents.DocumentRelation> builder)
    {
        builder.ToTable("DocumentRelations");
        builder.HasKey(e => e.Id);
        builder.HasOne<global::Energy.Domain.Modules.Documents.Document>().WithMany().HasForeignKey(e => e.DocumentId).OnDelete(DeleteBehavior.Cascade);
    }
}

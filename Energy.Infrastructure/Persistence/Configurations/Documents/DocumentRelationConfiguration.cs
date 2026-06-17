using Energy.Domain.Documents;
using Energy.Domain.IAM;
using Energy.Domain.Notifications;
using Energy.Domain.Reporting;
using Energy.Domain.Workflow;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.Documents;

/// <summary>DocumentRelation EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class DocumentRelationConfiguration : IEntityTypeConfiguration<DocumentRelation>
{
    public void Configure(EntityTypeBuilder<DocumentRelation> e)
    {
        e.ToTable("DocumentRelations");
        e.HasIndex(x => new { x.RelatedModule, x.RelatedEntityType, x.RelatedEntityId });
        e.HasOne<Document>().WithMany().HasForeignKey(x => x.DocumentId).OnDelete(DeleteBehavior.Cascade);
    }
}

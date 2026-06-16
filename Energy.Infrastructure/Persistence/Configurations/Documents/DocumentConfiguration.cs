using Energy.Domain.Documents;
using Energy.Domain.IAM;
using Energy.Domain.Notifications;
using Energy.Domain.Reporting;
using Energy.Domain.Workflow;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.Documents;

/// <summary>Document EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class DocumentConfiguration : IEntityTypeConfiguration<Document>
{
    public void Configure(EntityTypeBuilder<Document> e)
    {
        e.ToTable("Documents");
        e.HasOne<DocumentFolder>().WithMany().HasForeignKey(x => x.DocumentFolderId).OnDelete(DeleteBehavior.Restrict);
    }
}

using Energy.Domain.Modules.Documents;
using Energy.Domain.Modules.IAM;
using Energy.Domain.Modules.Notifications;
using Energy.Domain.Modules.Reporting;
using Energy.Domain.Modules.Workflow;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Documents;

/// <summary>Document EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class DocumentConfiguration : IEntityTypeConfiguration<Document>
{
    public void Configure(EntityTypeBuilder<Document> e)
    {
        e.ToTable("Documents");
        e.HasOne<DocumentFolder>().WithMany().HasForeignKey(x => x.DocumentFolderId).OnDelete(DeleteBehavior.Restrict);
    }
}

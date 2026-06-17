using Energy.Domain.Documents;
using Energy.Domain.IAM;
using Energy.Domain.Notifications;
using Energy.Domain.Reporting;
using Energy.Domain.Workflow;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.Documents;

/// <summary>DocumentFolder EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class DocumentFolderConfiguration : IEntityTypeConfiguration<DocumentFolder>
{
    public void Configure(EntityTypeBuilder<DocumentFolder> e)
    {
        e.ToTable("DocumentFolders");
        e.HasOne<DocumentFolder>().WithMany().HasForeignKey(x => x.ParentFolderId).OnDelete(DeleteBehavior.Restrict);
    }
}

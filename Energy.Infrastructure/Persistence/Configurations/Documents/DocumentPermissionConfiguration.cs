using Energy.Domain.Documents;
using Energy.Domain.IAM;
using Energy.Domain.Notifications;
using Energy.Domain.Reporting;
using Energy.Domain.Workflow;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.Documents;

/// <summary>DocumentPermission EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class DocumentPermissionConfiguration : IEntityTypeConfiguration<DocumentPermission>
{
    public void Configure(EntityTypeBuilder<DocumentPermission> e)
    {
        e.ToTable("DocumentPermissions");
        e.HasOne<Document>().WithMany().HasForeignKey(x => x.DocumentId).OnDelete(DeleteBehavior.Cascade);
        e.HasOne<User>().WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
        e.HasOne<Role>().WithMany().HasForeignKey(x => x.RoleId).OnDelete(DeleteBehavior.Restrict);
    }
}

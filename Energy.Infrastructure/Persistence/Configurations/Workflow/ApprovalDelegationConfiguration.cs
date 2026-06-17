using Energy.Domain.Documents;
using Energy.Domain.IAM;
using Energy.Domain.Notifications;
using Energy.Domain.Reporting;
using Energy.Domain.Workflow;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.Workflow;

/// <summary>ApprovalDelegation EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class ApprovalDelegationConfiguration : IEntityTypeConfiguration<ApprovalDelegation>
{
    public void Configure(EntityTypeBuilder<ApprovalDelegation> e)
    {
        e.ToTable("ApprovalDelegations");
        e.HasOne<User>().WithMany().HasForeignKey(x => x.DelegatorUserId).OnDelete(DeleteBehavior.Restrict);
        e.HasOne<User>().WithMany().HasForeignKey(x => x.DelegateUserId).OnDelete(DeleteBehavior.Restrict);
    }
}

using Energy.Domain.Documents;
using Energy.Domain.IAM;
using Energy.Domain.Notifications;
using Energy.Domain.Reporting;
using Energy.Domain.Workflow;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.Workflow;

/// <summary>ApprovalDefinition EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class ApprovalDefinitionConfiguration : IEntityTypeConfiguration<ApprovalDefinition>
{
    public void Configure(EntityTypeBuilder<ApprovalDefinition> e)
    {
        e.ToTable("ApprovalDefinitions"); e.HasIndex(x => x.Code).IsUnique(); 
    }
}

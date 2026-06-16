using Energy.Domain.Modules.Documents;
using Energy.Domain.Modules.IAM;
using Energy.Domain.Modules.Notifications;
using Energy.Domain.Modules.Reporting;
using Energy.Domain.Modules.Workflow;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Workflow;

/// <summary>ApprovalDefinition EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class ApprovalDefinitionConfiguration : IEntityTypeConfiguration<ApprovalDefinition>
{
    public void Configure(EntityTypeBuilder<ApprovalDefinition> e)
    {
        e.ToTable("ApprovalDefinitions"); e.HasIndex(x => x.Code).IsUnique(); 
    }
}

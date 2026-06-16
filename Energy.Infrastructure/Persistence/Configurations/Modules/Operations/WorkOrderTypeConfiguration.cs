using Energy.Domain.Modules.Assets;
using Energy.Domain.Modules.Catalog;
using Energy.Domain.Modules.Contracts;
using Energy.Domain.Modules.Core;
using Energy.Domain.Modules.FieldOperations;
using Energy.Domain.Modules.HR;
using Energy.Domain.Modules.IAM;
using Energy.Domain.Modules.Inventory;
using Energy.Domain.Modules.Operations;
using Energy.Domain.Modules.Organization;
using Energy.Domain.Modules.Projects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Operations;

/// <summary>WorkOrderType EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class WorkOrderTypeConfiguration : IEntityTypeConfiguration<WorkOrderType>
{
    public void Configure(EntityTypeBuilder<WorkOrderType> e)
    {
        e.ToTable("WorkOrderTypes"); e.HasIndex(x => x.Code).IsUnique(); 
    }
}

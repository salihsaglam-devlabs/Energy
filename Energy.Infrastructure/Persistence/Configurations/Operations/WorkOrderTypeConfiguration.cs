using Energy.Domain.Assets;
using Energy.Domain.Catalog;
using Energy.Domain.Contracts;
using Energy.Domain.Core;
using Energy.Domain.FieldOperations;
using Energy.Domain.HR;
using Energy.Domain.IAM;
using Energy.Domain.Inventory;
using Energy.Domain.Operations;
using Energy.Domain.Organization;
using Energy.Domain.Projects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.Operations;

/// <summary>WorkOrderType EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class WorkOrderTypeConfiguration : IEntityTypeConfiguration<WorkOrderType>
{
    public void Configure(EntityTypeBuilder<WorkOrderType> e)
    {
        e.ToTable("WorkOrderTypes"); e.HasIndex(x => x.Code).IsUnique(); 
    }
}

using Energy.Domain.Modules.BusinessPartners;
using Energy.Domain.Modules.Catalog;
using Energy.Domain.Modules.Core;
using Energy.Domain.Modules.IAM;
using Energy.Domain.Modules.Inventory;
using Energy.Domain.Modules.Procurement;
using Energy.Domain.Modules.Projects;
using Energy.Domain.Modules.Requests;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Requests;

/// <summary>RequestType EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class RequestTypeConfiguration : IEntityTypeConfiguration<RequestType>
{
    public void Configure(EntityTypeBuilder<RequestType> e)
    {
        e.ToTable("RequestTypes"); e.HasIndex(x => x.Code).IsUnique(); 
    }
}

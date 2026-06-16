using Energy.Domain.Modules.Catalog;
using Energy.Domain.Modules.Core;
using Energy.Domain.Modules.Inventory;
using Energy.Domain.Modules.Projects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Inventory;

/// <summary>StockDocumentType EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class StockDocumentTypeConfiguration : IEntityTypeConfiguration<StockDocumentType>
{
    public void Configure(EntityTypeBuilder<StockDocumentType> e)
    {
        e.ToTable("StockDocumentTypes"); e.HasIndex(x => x.Code).IsUnique(); 
    }
}

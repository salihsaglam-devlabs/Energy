using Energy.Domain.Catalog;
using Energy.Domain.Core;
using Energy.Domain.Inventory;
using Energy.Domain.Projects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Persistence.Configurations.Inventory;

/// <summary>StockDocumentType EF Core yapılandırması (Relationship Catalogue'a göre).</summary>
public sealed class StockDocumentTypeConfiguration : IEntityTypeConfiguration<StockDocumentType>
{
    public void Configure(EntityTypeBuilder<StockDocumentType> e)
    {
        e.ToTable("StockDocumentTypes"); e.HasIndex(x => x.Code).IsUnique(); 
    }
}

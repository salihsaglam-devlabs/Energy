using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Energy.Infrastructure.Persistence.Configurations.Modules.Inventory;

/// <summary>StockReservation EF Core eşleştirmesi (tablo, anahtar ve ilişkiler).</summary>
public class StockReservationConfiguration : IEntityTypeConfiguration<global::Energy.Domain.Modules.Inventory.StockReservation>
{
    public void Configure(EntityTypeBuilder<global::Energy.Domain.Modules.Inventory.StockReservation> builder)
    {
        builder.ToTable("StockReservations");
        builder.HasKey(e => e.Id);
    }
}

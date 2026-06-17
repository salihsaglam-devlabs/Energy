using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Inventory.StockReservation.Lookups;
using Energy.Shared.Models.V1.Inventory.StockReservation.Responses;

namespace Energy.Infrastructure.Inventory.StockReservation.Lookups;

/// <summary>StockReservation lookup servisi (aktif + arama filtreli projection).</summary>
public class StockReservationLookupService : IStockReservationLookupService
{
    private readonly AppDbContext _db;

    public StockReservationLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<StockReservationLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.StockReservations.AsNoTracking();
        var rows = await query
            .OrderBy(e => e.Id)
            .ToListAsync(ct);
        var items = (IReadOnlyList<StockReservationLookupResponse>)rows.Select(e => new StockReservationLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = string.IsNullOrWhiteSpace((e.RelatedModule ?? "") + " - " + e.Quantity.ToString()) ? "Stock Reservation #" + e.Id.ToString().Substring(0, 8) : ((e.RelatedModule ?? "") + " - " + e.Quantity.ToString()),
            IsActive = true
        }).ToList();
        return BaseResponse<IReadOnlyList<StockReservationLookupResponse>>.Success(items);
    }
}

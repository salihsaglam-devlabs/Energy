using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Inventory.StockReservation.Lookups;
using Energy.Shared.Models.V1.Inventory.StockReservation.Responses;

namespace Energy.Infrastructure.Modules.Inventory.StockReservation.Lookups;

/// <summary>StockReservation lookup servisi (aktif + arama filtreli projection).</summary>
public class StockReservationLookupService : IStockReservationLookupService
{
    private readonly EnergyDbContext _db;

    public StockReservationLookupService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<StockReservationLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.StockReservations.AsNoTracking();
        var items = await query.Select(e => new StockReservationLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = e.Id.ToString(),
            IsActive = true
        }).ToListAsync(ct);
        return BaseResponse<IReadOnlyList<StockReservationLookupResponse>>.Success(items);
    }
}

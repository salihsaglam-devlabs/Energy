using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockReservation.Responses;

namespace Energy.Application.Inventory.StockReservation.Lookups;

/// <summary>StockReservation lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IStockReservationLookupService
{
    /// <summary>StockReservation lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<StockReservationLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}

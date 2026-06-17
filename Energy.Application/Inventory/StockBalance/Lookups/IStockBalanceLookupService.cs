using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockBalance.Responses;

namespace Energy.Application.Inventory.StockBalance.Lookups;

/// <summary>StockBalance lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IStockBalanceLookupService
{
    /// <summary>StockBalance lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<StockBalanceLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}

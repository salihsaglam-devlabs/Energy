using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockCount.Responses;

namespace Energy.Application.Inventory.StockCount.Lookups;

/// <summary>StockCount lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IStockCountLookupService
{
    /// <summary>StockCount lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<StockCountLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}

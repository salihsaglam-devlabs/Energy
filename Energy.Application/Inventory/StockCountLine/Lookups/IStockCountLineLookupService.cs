using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockCountLine.Responses;

namespace Energy.Application.Inventory.StockCountLine.Lookups;

/// <summary>StockCountLine lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IStockCountLineLookupService
{
    /// <summary>StockCountLine lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<StockCountLineLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}

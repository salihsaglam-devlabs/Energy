using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockLot.Responses;

namespace Energy.Application.Modules.Inventory.StockLot.Lookups;

/// <summary>StockLot lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IStockLotLookupService
{
    /// <summary>StockLot lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<StockLotLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}

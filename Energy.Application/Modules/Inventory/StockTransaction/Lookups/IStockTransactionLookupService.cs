using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockTransaction.Responses;

namespace Energy.Application.Modules.Inventory.StockTransaction.Lookups;

/// <summary>StockTransaction lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IStockTransactionLookupService
{
    /// <summary>StockTransaction lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<StockTransactionLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}

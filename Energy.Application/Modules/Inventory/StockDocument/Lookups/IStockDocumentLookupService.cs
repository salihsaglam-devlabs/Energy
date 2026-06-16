using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockDocument.Responses;

namespace Energy.Application.Modules.Inventory.StockDocument.Lookups;

/// <summary>StockDocument lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IStockDocumentLookupService
{
    /// <summary>StockDocument lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<StockDocumentLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}

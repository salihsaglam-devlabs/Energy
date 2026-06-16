using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockDocumentType.Responses;

namespace Energy.Application.Modules.Inventory.StockDocumentType.Lookups;

/// <summary>StockDocumentType lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IStockDocumentTypeLookupService
{
    /// <summary>StockDocumentType lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<StockDocumentTypeLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}

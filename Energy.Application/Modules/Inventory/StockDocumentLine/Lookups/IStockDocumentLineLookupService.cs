using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockDocumentLine.Responses;

namespace Energy.Application.Modules.Inventory.StockDocumentLine.Lookups;

/// <summary>StockDocumentLine lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IStockDocumentLineLookupService
{
    /// <summary>StockDocumentLine lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<StockDocumentLineLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}

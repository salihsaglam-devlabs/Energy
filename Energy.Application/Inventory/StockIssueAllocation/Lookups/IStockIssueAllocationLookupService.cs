using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockIssueAllocation.Responses;

namespace Energy.Application.Inventory.StockIssueAllocation.Lookups;

/// <summary>StockIssueAllocation lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IStockIssueAllocationLookupService
{
    /// <summary>StockIssueAllocation lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<StockIssueAllocationLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}

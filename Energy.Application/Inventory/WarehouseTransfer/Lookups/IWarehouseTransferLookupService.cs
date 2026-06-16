using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.WarehouseTransfer.Responses;

namespace Energy.Application.Inventory.WarehouseTransfer.Lookups;

/// <summary>WarehouseTransfer lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IWarehouseTransferLookupService
{
    /// <summary>WarehouseTransfer lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<WarehouseTransferLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}

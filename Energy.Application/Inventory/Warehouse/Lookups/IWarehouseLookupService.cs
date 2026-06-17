using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.Warehouse.Responses;

namespace Energy.Application.Inventory.Warehouse.Lookups;

/// <summary>Warehouse lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IWarehouseLookupService
{
    /// <summary>Warehouse lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<WarehouseLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}

using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.WarehouseLocation.Responses;

namespace Energy.Application.Modules.Inventory.WarehouseLocation.Lookups;

/// <summary>WarehouseLocation lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IWarehouseLocationLookupService
{
    /// <summary>WarehouseLocation lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<WarehouseLocationLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}

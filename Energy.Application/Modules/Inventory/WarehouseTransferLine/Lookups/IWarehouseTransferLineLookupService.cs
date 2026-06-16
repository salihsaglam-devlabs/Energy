using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.WarehouseTransferLine.Responses;

namespace Energy.Application.Modules.Inventory.WarehouseTransferLine.Lookups;

/// <summary>WarehouseTransferLine lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IWarehouseTransferLineLookupService
{
    /// <summary>WarehouseTransferLine lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<WarehouseTransferLineLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}

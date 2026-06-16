using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Assets.EquipmentMaintenance.Responses;

namespace Energy.Application.Modules.Assets.EquipmentMaintenance.Lookups;

/// <summary>EquipmentMaintenance lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IEquipmentMaintenanceLookupService
{
    /// <summary>EquipmentMaintenance lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<EquipmentMaintenanceLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}

using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Assets.EquipmentAsset.Responses;

namespace Energy.Application.Assets.EquipmentAsset.Lookups;

/// <summary>EquipmentAsset lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IEquipmentAssetLookupService
{
    /// <summary>EquipmentAsset lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<EquipmentAssetLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}

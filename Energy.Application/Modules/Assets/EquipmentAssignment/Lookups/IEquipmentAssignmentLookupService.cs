using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Assets.EquipmentAssignment.Responses;

namespace Energy.Application.Modules.Assets.EquipmentAssignment.Lookups;

/// <summary>EquipmentAssignment lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IEquipmentAssignmentLookupService
{
    /// <summary>EquipmentAssignment lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<EquipmentAssignmentLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}

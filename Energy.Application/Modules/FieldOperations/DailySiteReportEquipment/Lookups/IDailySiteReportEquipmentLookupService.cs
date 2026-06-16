using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.FieldOperations.DailySiteReportEquipment.Responses;

namespace Energy.Application.Modules.FieldOperations.DailySiteReportEquipment.Lookups;

/// <summary>DailySiteReportEquipment lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IDailySiteReportEquipmentLookupService
{
    /// <summary>DailySiteReportEquipment lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<DailySiteReportEquipmentLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}

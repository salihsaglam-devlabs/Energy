using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.FieldOperations.DailySiteReportMaterial.Responses;

namespace Energy.Application.FieldOperations.DailySiteReportMaterial.Lookups;

/// <summary>DailySiteReportMaterial lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IDailySiteReportMaterialLookupService
{
    /// <summary>DailySiteReportMaterial lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<DailySiteReportMaterialLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}

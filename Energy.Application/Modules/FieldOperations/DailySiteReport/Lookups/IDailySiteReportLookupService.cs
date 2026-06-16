using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.FieldOperations.DailySiteReport.Responses;

namespace Energy.Application.Modules.FieldOperations.DailySiteReport.Lookups;

/// <summary>DailySiteReport lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IDailySiteReportLookupService
{
    /// <summary>DailySiteReport lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<DailySiteReportLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}

using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.FieldOperations.DailySiteReportWorker.Responses;

namespace Energy.Application.FieldOperations.DailySiteReportWorker.Lookups;

/// <summary>DailySiteReportWorker lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IDailySiteReportWorkerLookupService
{
    /// <summary>DailySiteReportWorker lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<DailySiteReportWorkerLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}

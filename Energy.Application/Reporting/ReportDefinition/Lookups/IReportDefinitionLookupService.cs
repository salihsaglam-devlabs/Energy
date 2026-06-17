using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Reporting.ReportDefinition.Responses;

namespace Energy.Application.Reporting.ReportDefinition.Lookups;

/// <summary>ReportDefinition lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IReportDefinitionLookupService
{
    /// <summary>ReportDefinition lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<ReportDefinitionLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}

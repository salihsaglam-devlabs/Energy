using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Reporting.DashboardWidget.Responses;

namespace Energy.Application.Modules.Reporting.DashboardWidget.Lookups;

/// <summary>DashboardWidget lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IDashboardWidgetLookupService
{
    /// <summary>DashboardWidget lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<DashboardWidgetLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}

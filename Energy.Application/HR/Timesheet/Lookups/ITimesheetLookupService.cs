using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.HR.Timesheet.Responses;

namespace Energy.Application.HR.Timesheet.Lookups;

/// <summary>Timesheet lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface ITimesheetLookupService
{
    /// <summary>Timesheet lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<TimesheetLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}

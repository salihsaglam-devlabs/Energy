using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.HR.TimesheetLine.Responses;

namespace Energy.Application.HR.TimesheetLine.Lookups;

/// <summary>TimesheetLine lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface ITimesheetLineLookupService
{
    /// <summary>TimesheetLine lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<TimesheetLineLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}

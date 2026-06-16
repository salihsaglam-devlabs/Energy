using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Organization.LeaveRequest.Responses;

namespace Energy.Application.Modules.Organization.LeaveRequest.Lookups;

/// <summary>LeaveRequest lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface ILeaveRequestLookupService
{
    /// <summary>LeaveRequest lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<LeaveRequestLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}

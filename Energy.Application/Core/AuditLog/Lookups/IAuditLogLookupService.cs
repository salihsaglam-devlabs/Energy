using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.AuditLog.Responses;

namespace Energy.Application.Core.AuditLog.Lookups;

/// <summary>AuditLog lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IAuditLogLookupService
{
    /// <summary>AuditLog lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<AuditLogLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}

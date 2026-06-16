using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.IAM.Permission.Responses;

namespace Energy.Application.Modules.IAM.Permission.Lookups;

/// <summary>Permission lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IPermissionLookupService
{
    /// <summary>Permission lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<PermissionLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}

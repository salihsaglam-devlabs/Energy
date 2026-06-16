using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.IAM.RolePermission.Responses;

namespace Energy.Application.Modules.IAM.RolePermission.Lookups;

/// <summary>RolePermission lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IRolePermissionLookupService
{
    /// <summary>RolePermission lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<RolePermissionLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}

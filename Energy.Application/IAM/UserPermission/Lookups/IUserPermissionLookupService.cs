using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.IAM.UserPermission.Responses;

namespace Energy.Application.IAM.UserPermission.Lookups;

/// <summary>UserPermission lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IUserPermissionLookupService
{
    /// <summary>UserPermission lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<UserPermissionLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}

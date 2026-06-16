using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.IAM.UserRole.Responses;

namespace Energy.Application.IAM.UserRole.Lookups;

/// <summary>UserRole lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IUserRoleLookupService
{
    /// <summary>UserRole lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<UserRoleLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}

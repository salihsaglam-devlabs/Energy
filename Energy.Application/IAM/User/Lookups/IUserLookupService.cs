using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.IAM.User.Responses;

namespace Energy.Application.IAM.User.Lookups;

/// <summary>User lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IUserLookupService
{
    /// <summary>User lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<UserLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}

using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.IAM.Role.Responses;

namespace Energy.Application.IAM.Role.Lookups;

/// <summary>Role lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IRoleLookupService
{
    /// <summary>Role lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<RoleLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}

using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.IAM.Menu.Responses;

namespace Energy.Application.IAM.Menu.Lookups;

/// <summary>Menu lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IMenuLookupService
{
    /// <summary>Menu lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<MenuLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}

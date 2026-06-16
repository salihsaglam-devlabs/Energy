using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.CostCenter.Responses;

namespace Energy.Application.Finance.CostCenter.Lookups;

/// <summary>CostCenter lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface ICostCenterLookupService
{
    /// <summary>CostCenter lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<CostCenterLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}

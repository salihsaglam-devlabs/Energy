using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.Company.Responses;

namespace Energy.Application.Modules.Core.Company.Lookups;

/// <summary>Company lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface ICompanyLookupService
{
    /// <summary>Company lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<CompanyLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}

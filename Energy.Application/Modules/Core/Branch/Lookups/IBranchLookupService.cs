using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.Branch.Responses;

namespace Energy.Application.Modules.Core.Branch.Lookups;

/// <summary>Branch lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IBranchLookupService
{
    /// <summary>Branch lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<BranchLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}

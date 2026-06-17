using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.Department.Responses;

namespace Energy.Application.Core.Department.Lookups;

/// <summary>Department lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IDepartmentLookupService
{
    /// <summary>Department lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<DepartmentLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}

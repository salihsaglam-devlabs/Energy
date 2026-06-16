using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Organization.EmployeePosition.Responses;

namespace Energy.Application.Organization.EmployeePosition.Lookups;

/// <summary>EmployeePosition lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IEmployeePositionLookupService
{
    /// <summary>EmployeePosition lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<EmployeePositionLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}

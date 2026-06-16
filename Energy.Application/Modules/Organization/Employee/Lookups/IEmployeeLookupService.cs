using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Organization.Employee.Responses;

namespace Energy.Application.Modules.Organization.Employee.Lookups;

/// <summary>Employee lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IEmployeeLookupService
{
    /// <summary>Employee lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<EmployeeLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}

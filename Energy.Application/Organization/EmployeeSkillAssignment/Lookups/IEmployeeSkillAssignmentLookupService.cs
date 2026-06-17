using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Organization.EmployeeSkillAssignment.Responses;

namespace Energy.Application.Organization.EmployeeSkillAssignment.Lookups;

/// <summary>EmployeeSkillAssignment lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IEmployeeSkillAssignmentLookupService
{
    /// <summary>EmployeeSkillAssignment lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<EmployeeSkillAssignmentLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}

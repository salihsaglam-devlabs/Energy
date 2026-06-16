using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Organization.EmployeeSkill.Responses;

namespace Energy.Application.Modules.Organization.EmployeeSkill.Lookups;

/// <summary>EmployeeSkill lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IEmployeeSkillLookupService
{
    /// <summary>EmployeeSkill lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<EmployeeSkillLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}

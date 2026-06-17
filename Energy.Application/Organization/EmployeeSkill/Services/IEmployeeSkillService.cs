using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Organization.EmployeeSkill.Requests;
using Energy.Shared.Models.V1.Organization.EmployeeSkill.Responses;

namespace Energy.Application.Organization.EmployeeSkill.Services;

/// <summary>EmployeeSkill CRUD use-case sözleşmesi.</summary>
public interface IEmployeeSkillService
{
    /// <summary>Sayfalanmış EmployeeSkill listesi.</summary>
    Task<BaseResponse<PaginatedResponse<EmployeeSkillListResponse>>> GetListAsync(GetEmployeeSkillListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<EmployeeSkillDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateEmployeeSkillRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateEmployeeSkillRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}

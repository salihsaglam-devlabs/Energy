using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Organization.EmployeeSkillAssignment.Requests;
using Energy.Shared.Models.V1.Organization.EmployeeSkillAssignment.Responses;

namespace Energy.Application.Organization.EmployeeSkillAssignment.Services;

/// <summary>EmployeeSkillAssignment CRUD use-case sözleşmesi.</summary>
public interface IEmployeeSkillAssignmentService
{
    /// <summary>Sayfalanmış EmployeeSkillAssignment listesi.</summary>
    Task<BaseResponse<PaginatedResponse<EmployeeSkillAssignmentListResponse>>> GetListAsync(GetEmployeeSkillAssignmentListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<EmployeeSkillAssignmentDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateEmployeeSkillAssignmentRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateEmployeeSkillAssignmentRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}

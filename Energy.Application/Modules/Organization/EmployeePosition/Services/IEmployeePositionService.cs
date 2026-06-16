using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Organization.EmployeePosition.Requests;
using Energy.Shared.Models.V1.Organization.EmployeePosition.Responses;

namespace Energy.Application.Modules.Organization.EmployeePosition.Services;

/// <summary>EmployeePosition CRUD use-case sözleşmesi.</summary>
public interface IEmployeePositionService
{
    /// <summary>Sayfalanmış EmployeePosition listesi.</summary>
    Task<BaseResponse<PaginatedResponse<EmployeePositionListResponse>>> GetListAsync(GetEmployeePositionListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<EmployeePositionDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateEmployeePositionRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateEmployeePositionRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}

using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.Department.Requests;
using Energy.Shared.Models.V1.Core.Department.Responses;

namespace Energy.Application.Core.Department.Services;

/// <summary>Department CRUD use-case sözleşmesi.</summary>
public interface IDepartmentService
{
    /// <summary>Sayfalanmış Department listesi.</summary>
    Task<BaseResponse<PaginatedResponse<DepartmentListResponse>>> GetListAsync(GetDepartmentListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<DepartmentDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateDepartmentRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateDepartmentRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}

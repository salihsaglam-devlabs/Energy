using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Projects.ProjectType.Requests;
using Energy.Shared.Models.V1.Projects.ProjectType.Responses;

namespace Energy.Application.Projects.ProjectType.Services;

/// <summary>ProjectType CRUD use-case sözleşmesi.</summary>
public interface IProjectTypeService
{
    /// <summary>Sayfalanmış ProjectType listesi.</summary>
    Task<BaseResponse<PaginatedResponse<ProjectTypeListResponse>>> GetListAsync(GetProjectTypeListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<ProjectTypeDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateProjectTypeRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateProjectTypeRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}

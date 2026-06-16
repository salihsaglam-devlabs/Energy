using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Projects.ProjectStatus.Requests;
using Energy.Shared.Models.V1.Projects.ProjectStatus.Responses;

namespace Energy.Application.Projects.ProjectStatus.Services;

/// <summary>ProjectStatus CRUD use-case sözleşmesi.</summary>
public interface IProjectStatusService
{
    /// <summary>Sayfalanmış ProjectStatus listesi.</summary>
    Task<BaseResponse<PaginatedResponse<ProjectStatusListResponse>>> GetListAsync(GetProjectStatusListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<ProjectStatusDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateProjectStatusRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateProjectStatusRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}

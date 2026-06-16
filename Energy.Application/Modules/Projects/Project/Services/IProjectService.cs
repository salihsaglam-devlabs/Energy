using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Projects.Project.Requests;
using Energy.Shared.Models.V1.Projects.Project.Responses;

namespace Energy.Application.Modules.Projects.Project.Services;

/// <summary>Project CRUD use-case sözleşmesi.</summary>
public interface IProjectService
{
    /// <summary>Sayfalanmış Project listesi.</summary>
    Task<BaseResponse<PaginatedResponse<ProjectListResponse>>> GetListAsync(GetProjectListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<ProjectDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateProjectRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateProjectRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}

using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Projects.ProjectLocation.Requests;
using Energy.Shared.Models.V1.Projects.ProjectLocation.Responses;

namespace Energy.Application.Projects.ProjectLocation.Services;

/// <summary>ProjectLocation CRUD use-case sözleşmesi.</summary>
public interface IProjectLocationService
{
    /// <summary>Sayfalanmış ProjectLocation listesi.</summary>
    Task<BaseResponse<PaginatedResponse<ProjectLocationListResponse>>> GetListAsync(GetProjectLocationListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<ProjectLocationDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateProjectLocationRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateProjectLocationRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}

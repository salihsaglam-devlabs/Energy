using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Projects.ProjectPhas.Requests;
using Energy.Shared.Models.V1.Projects.ProjectPhas.Responses;

namespace Energy.Application.Modules.Projects.ProjectPhas.Services;

/// <summary>ProjectPhas CRUD use-case sözleşmesi.</summary>
public interface IProjectPhasService
{
    /// <summary>Sayfalanmış ProjectPhas listesi.</summary>
    Task<BaseResponse<PaginatedResponse<ProjectPhasListResponse>>> GetListAsync(GetProjectPhasListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<ProjectPhasDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateProjectPhasRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateProjectPhasRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}

using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Projects.ProjectNote.Requests;
using Energy.Shared.Models.V1.Projects.ProjectNote.Responses;

namespace Energy.Application.Modules.Projects.ProjectNote.Services;

/// <summary>ProjectNote CRUD use-case sözleşmesi.</summary>
public interface IProjectNoteService
{
    /// <summary>Sayfalanmış ProjectNote listesi.</summary>
    Task<BaseResponse<PaginatedResponse<ProjectNoteListResponse>>> GetListAsync(GetProjectNoteListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<ProjectNoteDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateProjectNoteRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateProjectNoteRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}

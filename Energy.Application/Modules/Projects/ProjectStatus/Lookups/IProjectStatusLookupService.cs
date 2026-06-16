using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Projects.ProjectStatus.Responses;

namespace Energy.Application.Modules.Projects.ProjectStatus.Lookups;

/// <summary>ProjectStatus lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IProjectStatusLookupService
{
    /// <summary>ProjectStatus lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<ProjectStatusLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}

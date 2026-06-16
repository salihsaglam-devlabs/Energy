using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Projects.ProjectLocation.Responses;

namespace Energy.Application.Modules.Projects.ProjectLocation.Lookups;

/// <summary>ProjectLocation lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IProjectLocationLookupService
{
    /// <summary>ProjectLocation lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<ProjectLocationLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}

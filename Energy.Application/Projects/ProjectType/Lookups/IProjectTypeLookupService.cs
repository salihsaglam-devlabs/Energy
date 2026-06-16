using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Projects.ProjectType.Responses;

namespace Energy.Application.Projects.ProjectType.Lookups;

/// <summary>ProjectType lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IProjectTypeLookupService
{
    /// <summary>ProjectType lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<ProjectTypeLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}

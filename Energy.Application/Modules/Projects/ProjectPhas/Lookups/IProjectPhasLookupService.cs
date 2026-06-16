using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Projects.ProjectPhas.Responses;

namespace Energy.Application.Modules.Projects.ProjectPhas.Lookups;

/// <summary>ProjectPhas lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IProjectPhasLookupService
{
    /// <summary>ProjectPhas lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<ProjectPhasLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}

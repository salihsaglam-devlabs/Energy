using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Projects.Project.Responses;

namespace Energy.Application.Modules.Projects.Project.Lookups;

/// <summary>Project lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IProjectLookupService
{
    /// <summary>Project lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<ProjectLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}

using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Projects.ProjectMember.Responses;

namespace Energy.Application.Projects.ProjectMember.Lookups;

/// <summary>ProjectMember lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IProjectMemberLookupService
{
    /// <summary>ProjectMember lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<ProjectMemberLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}

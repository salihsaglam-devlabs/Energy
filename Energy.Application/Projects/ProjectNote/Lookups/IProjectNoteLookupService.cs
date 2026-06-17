using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Projects.ProjectNote.Responses;

namespace Energy.Application.Projects.ProjectNote.Lookups;

/// <summary>ProjectNote lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IProjectNoteLookupService
{
    /// <summary>ProjectNote lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<ProjectNoteLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}

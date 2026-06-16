using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Projects.ProjectLocation.Lookups;
using Energy.Shared.Models.V1.Projects.ProjectLocation.Responses;

namespace Energy.Infrastructure.Projects.ProjectLocation.Lookups;

/// <summary>ProjectLocation lookup servisi (aktif + arama filtreli projection).</summary>
public class ProjectLocationLookupService : IProjectLocationLookupService
{
    private readonly AppDbContext _db;

    public ProjectLocationLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<ProjectLocationLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.ProjectLocations.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(search)) query = query.Where(e => e.Name.Contains(search) || e.Code.Contains(search));
        var items = await query
            .OrderBy(e => e.Name)
            .Select(e => new ProjectLocationLookupResponse
            {
                Id = e.Id,
                Code = e.Code,
                Name = e.Name,
                DisplayName = e.Code + " - " + e.Name,
                IsActive = true
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<ProjectLocationLookupResponse>>.Success(items);
    }
}

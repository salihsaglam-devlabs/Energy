using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Projects.ProjectStatus.Lookups;
using Energy.Shared.Models.V1.Projects.ProjectStatus.Responses;

namespace Energy.Infrastructure.Projects.ProjectStatus.Lookups;

/// <summary>ProjectStatus lookup servisi (aktif + arama filtreli projection).</summary>
public class ProjectStatusLookupService : IProjectStatusLookupService
{
    private readonly AppDbContext _db;

    public ProjectStatusLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<ProjectStatusLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.ProjectStatuses.AsNoTracking();
        if (activeOnly) query = query.Where(e => e.IsActive);
        if (!string.IsNullOrWhiteSpace(search)) query = query.Where(e => e.Name.Contains(search) || e.Code.Contains(search));
        var items = await query
            .OrderBy(e => e.Name)
            .Select(e => new ProjectStatusLookupResponse
            {
                Id = e.Id,
                Code = e.Code,
                Name = e.Name,
                DisplayName = e.Code + " - " + e.Name,
                IsActive = e.IsActive
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<ProjectStatusLookupResponse>>.Success(items);
    }
}

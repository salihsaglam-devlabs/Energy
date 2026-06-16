using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Projects.ProjectType.Lookups;
using Energy.Shared.Models.V1.Projects.ProjectType.Responses;

namespace Energy.Infrastructure.Projects.ProjectType.Lookups;

/// <summary>ProjectType lookup servisi (aktif + arama filtreli projection).</summary>
public class ProjectTypeLookupService : IProjectTypeLookupService
{
    private readonly AppDbContext _db;

    public ProjectTypeLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<ProjectTypeLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.ProjectTypes.AsNoTracking();
        if (activeOnly) query = query.Where(e => e.IsActive);
        if (!string.IsNullOrWhiteSpace(search)) query = query.Where(e => e.Name.Contains(search) || e.Code.Contains(search));
        var items = await query
            .OrderBy(e => e.Name)
            .Select(e => new ProjectTypeLookupResponse
            {
                Id = e.Id,
                Code = e.Code,
                Name = e.Name,
                DisplayName = e.Code + " - " + e.Name,
                IsActive = e.IsActive
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<ProjectTypeLookupResponse>>.Success(items);
    }
}

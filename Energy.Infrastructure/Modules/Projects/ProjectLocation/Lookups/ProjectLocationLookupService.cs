using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Projects.ProjectLocation.Lookups;
using Energy.Shared.Models.V1.Projects.ProjectLocation.Responses;

namespace Energy.Infrastructure.Modules.Projects.ProjectLocation.Lookups;

/// <summary>ProjectLocation lookup servisi (aktif + arama filtreli projection).</summary>
public class ProjectLocationLookupService : IProjectLocationLookupService
{
    private readonly EnergyDbContext _db;

    public ProjectLocationLookupService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<ProjectLocationLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.ProjectLocations.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(search)) query = query.Where(e => e.Name.Contains(search));
        var items = await query.Select(e => new ProjectLocationLookupResponse
        {
            Id = e.Id,
            Code = e.Code,
            Name = e.Name,
            DisplayName = e.Name,
            IsActive = true
        }).ToListAsync(ct);
        return BaseResponse<IReadOnlyList<ProjectLocationLookupResponse>>.Success(items);
    }
}

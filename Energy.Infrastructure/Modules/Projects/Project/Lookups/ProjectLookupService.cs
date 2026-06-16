using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Projects.Project.Lookups;
using Energy.Shared.Models.V1.Projects.Project.Responses;

namespace Energy.Infrastructure.Modules.Projects.Project.Lookups;

/// <summary>Project lookup servisi (aktif + arama filtreli projection).</summary>
public class ProjectLookupService : IProjectLookupService
{
    private readonly EnergyDbContext _db;

    public ProjectLookupService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<ProjectLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.Projects.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(search)) query = query.Where(e => e.Name.Contains(search));
        var items = await query.Select(e => new ProjectLookupResponse
        {
            Id = e.Id,
            Code = e.Code,
            Name = e.Name,
            DisplayName = e.Name,
            IsActive = true
        }).ToListAsync(ct);
        return BaseResponse<IReadOnlyList<ProjectLookupResponse>>.Success(items);
    }
}

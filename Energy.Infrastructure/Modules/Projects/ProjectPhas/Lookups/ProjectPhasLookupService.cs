using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Projects.ProjectPhas.Lookups;
using Energy.Shared.Models.V1.Projects.ProjectPhas.Responses;

namespace Energy.Infrastructure.Modules.Projects.ProjectPhas.Lookups;

/// <summary>ProjectPhas lookup servisi (aktif + arama filtreli projection).</summary>
public class ProjectPhasLookupService : IProjectPhasLookupService
{
    private readonly EnergyDbContext _db;

    public ProjectPhasLookupService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<ProjectPhasLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.ProjectPhases.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(search)) query = query.Where(e => e.Name.Contains(search));
        var items = await query.Select(e => new ProjectPhasLookupResponse
        {
            Id = e.Id,
            Code = e.Code,
            Name = e.Name,
            DisplayName = e.Name,
            IsActive = true
        }).ToListAsync(ct);
        return BaseResponse<IReadOnlyList<ProjectPhasLookupResponse>>.Success(items);
    }
}

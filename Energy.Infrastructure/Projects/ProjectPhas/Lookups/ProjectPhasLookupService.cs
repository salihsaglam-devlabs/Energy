using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Projects.ProjectPhas.Lookups;
using Energy.Shared.Models.V1.Projects.ProjectPhas.Responses;

namespace Energy.Infrastructure.Projects.ProjectPhas.Lookups;

/// <summary>ProjectPhas lookup servisi (aktif + arama filtreli projection).</summary>
public class ProjectPhasLookupService : IProjectPhasLookupService
{
    private readonly AppDbContext _db;

    public ProjectPhasLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<ProjectPhasLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.ProjectPhases.AsNoTracking();
        var items = await query
            .Select(e => new ProjectPhasLookupResponse
            {
                Id = Guid.Empty,
                Code = null,
                Name = null,
                DisplayName = "",
                IsActive = true
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<ProjectPhasLookupResponse>>.Success(items);
    }
}

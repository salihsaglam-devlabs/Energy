using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Projects.ProjectNote.Lookups;
using Energy.Shared.Models.V1.Projects.ProjectNote.Responses;

namespace Energy.Infrastructure.Modules.Projects.ProjectNote.Lookups;

/// <summary>ProjectNote lookup servisi (aktif + arama filtreli projection).</summary>
public class ProjectNoteLookupService : IProjectNoteLookupService
{
    private readonly EnergyDbContext _db;

    public ProjectNoteLookupService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<ProjectNoteLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.ProjectNotes.AsNoTracking();
        var items = await query.Select(e => new ProjectNoteLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = e.Id.ToString(),
            IsActive = true
        }).ToListAsync(ct);
        return BaseResponse<IReadOnlyList<ProjectNoteLookupResponse>>.Success(items);
    }
}

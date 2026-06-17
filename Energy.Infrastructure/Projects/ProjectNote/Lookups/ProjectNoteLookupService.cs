using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Projects.ProjectNote.Lookups;
using Energy.Shared.Models.V1.Projects.ProjectNote.Responses;

namespace Energy.Infrastructure.Projects.ProjectNote.Lookups;

/// <summary>ProjectNote lookup servisi (aktif + arama filtreli projection).</summary>
public class ProjectNoteLookupService : IProjectNoteLookupService
{
    private readonly AppDbContext _db;

    public ProjectNoteLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<ProjectNoteLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.ProjectNotes.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(search)) query = query.Where(e => e.Title.Contains(search));
        var items = await query
            .OrderBy(e => e.Title)
            .Select(e => new ProjectNoteLookupResponse
            {
                Id = e.Id,
                Code = null,
                Name = e.Title,
                DisplayName = e.Title,
                IsActive = true
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<ProjectNoteLookupResponse>>.Success(items);
    }
}

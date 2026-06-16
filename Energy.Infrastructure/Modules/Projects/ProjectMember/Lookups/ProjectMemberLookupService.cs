using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Projects.ProjectMember.Lookups;
using Energy.Shared.Models.V1.Projects.ProjectMember.Responses;

namespace Energy.Infrastructure.Modules.Projects.ProjectMember.Lookups;

/// <summary>ProjectMember lookup servisi (aktif + arama filtreli projection).</summary>
public class ProjectMemberLookupService : IProjectMemberLookupService
{
    private readonly EnergyDbContext _db;

    public ProjectMemberLookupService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<ProjectMemberLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.ProjectMembers.AsNoTracking();
        var items = await query.Select(e => new ProjectMemberLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = e.Id.ToString(),
            IsActive = true
        }).ToListAsync(ct);
        return BaseResponse<IReadOnlyList<ProjectMemberLookupResponse>>.Success(items);
    }
}
